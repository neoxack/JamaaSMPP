/************************************************************************
 * Copyright (C) 2007 Jamaa Technologies
 *
 * This file is part of Jamaa SMPP Library.
 *
 * Jamaa SMPP Library is free software. You can redistribute it and/or modify
 * it under the terms of the Microsoft Reciprocal License (Ms-RL)
 *
 * You should have received a copy of the Microsoft Reciprocal License
 * along with Jamaa SMPP Library; See License.txt for more details.
 *
 * Author: Benedict J. Tesha
 * benedict.tesha@jamaatech.com, www.jamaatech.com
 *
 ************************************************************************/

using System;
using JamaaTech.Smpp.Net.Lib.Networking;
using JamaaTech.Smpp.Net.Lib.Protocol;
using System.Timers;
using System.Net;
using System.Diagnostics;
using JamaaTech.Smpp.Net.Lib.Util;

namespace JamaaTech.Smpp.Net.Lib
{
    public class SmppClientSession
    {
        #region Variables
        private Timer _vTimer;
        private PduTransmitter _vTrans;
        private ResponseHandler _vRespHandler;
        private StreamParser _vStreamParser;
        private TcpIpSession _vTcpIpSession;
        private object _vSyncRoot;
        private bool _vIsAlive;
        private SmppSessionState _vState;
        private int _vDefaultResponseTimeout;

        private string _vSmscId;
        private string _vSystemId;
        private string _vPassword;
        private TypeOfNumber _vAddressTon;
        private NumberingPlanIndicator _vAddressNpi;

        private readonly SendPduCallback _vCallback;
        //--
        private static readonly TraceSwitch _vTraceSwitch = 
            new TraceSwitch("SmppClientSessionSwitch", "SmppClientSession class switch");
        #endregion

        #region Constants
        /// <summary>
        /// Default delay between consecutive EnquireLink commands
        /// </summary>
        private const int DefaultDelay = 60000;
        #endregion

        #region Events
        public event EventHandler<PduReceivedEventArgs> PduReceived;
        public event EventHandler<SmppSessionClosedEventArgs> SessionClosed;
        #endregion

        #region Constructors
        private SmppClientSession()
        {
            InitializeTimer();
            _vSyncRoot = new object();
            _vDefaultResponseTimeout = 5000;
            _vSmscId = "";
            _vSystemId = "";
            _vPassword = "";
            _vCallback = new SendPduCallback(SendPdu);
            //-- Create and initialize trace switch
        }
        #endregion

        #region Properties
        public bool IsAlive
        {
            get { lock (_vSyncRoot) { return _vIsAlive; } }
        }

        public SmppSessionState State
        {
            get { lock (_vSyncRoot) { return _vState; } }
        }

        public string SmscId => _vSmscId;

        public string SystemId => _vSystemId;

        public string Password => _vPassword;

        public NumberingPlanIndicator AddressNpi => _vAddressNpi;

        public TypeOfNumber AddressTon => _vAddressTon;

        public int EnquireLinkInterval
        {
            get { return (int)_vTimer.Interval; }
            set
            {
                if (value < 1000)//If the value is less than one second
                {
                    throw new ArgumentException("EnqureLink interval cannot be less than 1000 millseconds (1 second)");
                }
                _vTimer.Interval = (double)value;
            }
        }

        public TcpIpSessionProperties TcpIpProperties => _vTcpIpSession.Properties;

        public int DefaultResponseTimeout
        {
            get { return _vDefaultResponseTimeout; }
            set { _vDefaultResponseTimeout = value; }
        }

        public object SyncRoot
        {
            get { return _vSyncRoot; }
            set { _vSyncRoot = value; }
        }
        #endregion

        #region Methods
        #region Interface Methods
        public ResponsePdu SendPdu(RequestPdu pdu)
        {
            int timeout = 0;
            lock (_vSyncRoot) { timeout = _vDefaultResponseTimeout; }
            return SendPdu(pdu, timeout);
        }

        public ResponsePdu SendPdu(RequestPdu pdu, int timeout)
        {
            SendPduBase(pdu);
            if (pdu.HasResponse) 
            {
                try { return _vRespHandler.WaitResponse(pdu, timeout); }
                catch (SmppResponseTimedOutException)
                {
                    if (_vTraceSwitch.TraceWarning)
                    { Trace.WriteLine("200016:PDU send operation timed out;"); }
                    throw;
                }
            }
            else { return null; }
        }

        private void SendPduBase(Pdu pdu)
        {
            if (pdu == null) { throw new ArgumentNullException("pdu"); }
            if (!(CheckState(pdu) && (pdu.AllowedSource & SmppEntityType.Esme) == SmppEntityType.Esme))
            { throw new SmppException(SmppErrorCode.EsmeRinvbndsts, "Incorrect bind status for given command"); }
            try { _vTrans.Send(pdu); }
            catch (Exception ex)
            {
                if (_vTraceSwitch.TraceInfo)
                {
                    ByteBuffer buffer = new ByteBuffer(pdu.GetBytes());
                    Trace.WriteLine(string.Format(
                        "200022:PDU send operation failed - {0} {1};",
                        buffer.DumpString(), ex.Message));
                }
            }
        }

        public IAsyncResult BeginSendPdu(RequestPdu pdu, int timeout,AsyncCallback callback, object @object)
        {
            return _vCallback.BeginInvoke(pdu, timeout, callback, @object);
        }

        public IAsyncResult BeginSendPdu(RequestPdu pdu, AsyncCallback callback, object @object)
        {
            int timeout = 0;
            lock (_vSyncRoot) { timeout = _vDefaultResponseTimeout; }
            return BeginSendPdu(pdu, timeout, callback, @object);
        }

        public ResponsePdu EndSendPdu(IAsyncResult result)
        {
            return _vCallback.EndInvoke(result);
        }

        public void EndSession()
        {
            EndSession(SmppSessionCloseReason.EndSessionCalled, null);
        }

        public static SmppClientSession Bind(SessionBindInfo bindInfo, int timeOut)
        {
            try
            {
                TcpIpSession tcpIpSession = null;
                if (bindInfo == null) { throw new ArgumentNullException("bindInfo"); }
                //--
                tcpIpSession = CreateTcpIpSession(bindInfo);
                //--
                SmppClientSession smppSession = new SmppClientSession();
                smppSession._vTcpIpSession = tcpIpSession;
                smppSession.ChangeState(SmppSessionState.Open);
                smppSession.AssembleComponents();
                try { smppSession.BindSession(bindInfo, timeOut); }
                catch (Exception)
                {
                    smppSession.DestroyTcpIpSession();
                    smppSession.DisassembleComponents();
                    throw;
                }
                return smppSession;
            }
            catch (Exception ex)
            {
                if(_vTraceSwitch.TraceInfo)
                {
                    string traceMessage = "200017:SMPP bind operation failed:";
                    if(ex is SmppException) { traceMessage += (ex as SmppException).ErrorCode.ToString() + " - "; }
                    traceMessage += ex.Message;
                    Trace.WriteLine(traceMessage);
                }
                throw;
            }
        }
        #endregion

        #region Helper Methods
        private void EndSession(SmppSessionCloseReason reason, Exception exception)
        {
            lock (_vSyncRoot)
            {
                if (!_vIsAlive) { return; }
                _vIsAlive = false;
                ChangeState(SmppSessionState.Closed);
            }
            if (reason != SmppSessionCloseReason.UnbindRequested)
            {
                //If unbind request was received, do not try to unbind again
                Unbind unbind = new Unbind();
                try 
                {
                    _vTrans.Send(unbind);
                    _vRespHandler.WaitResponse(unbind, 1000);
                }
                catch {/*Silent catch*/}
            }
            DestroyTcpIpSession();
            DisassembleComponents();
            DestroyTimer();
            RaiseSessionClosedEvent(reason, exception);
        }

        private static TcpIpSession CreateTcpIpSession(SessionBindInfo bindInfo)
        {
            //Check that Host is not an empty string or null
            if (string.IsNullOrEmpty(bindInfo.ServerName)) 
            { throw new InvalidOperationException("Host cannot be an empty string or null"); }
            //Check the port number is not set to an invalid value
            if (bindInfo.Port < IPEndPoint.MinPort || bindInfo.Port > IPEndPoint.MaxPort)
            {
                throw new InvalidOperationException(
                   string.Format("Port must be set to a valid value between '{0}' and '{1}'",
                   IPEndPoint.MinPort, IPEndPoint.MaxPort));
            }
            IPAddress address = null;
            TcpIpSession session = null;
            if (!IPAddress.TryParse(bindInfo.ServerName, out address))
            { session = TcpIpSession.OpenClientSession(bindInfo.ServerName, bindInfo.Port); }
            else { session = TcpIpSession.OpenClientSession(address, bindInfo.Port); }
            return session;
        }

        private void DestroyTcpIpSession()
        {
            if (_vTcpIpSession == null) { return; }
            _vTcpIpSession.SessionClosed -= TcpIpSessionClosedEventHandler;
            _vTcpIpSession.EndSession();
            _vTcpIpSession = null;
        }

        private void AssembleComponents()
        {
            _vTrans = new PduTransmitter(_vTcpIpSession);
            _vRespHandler = new ResponseHandler();
            _vStreamParser = new StreamParser(
                _vTcpIpSession, _vRespHandler, new PduProcessorCallback(PduRequestProcessorCallback));
            _vStreamParser.ParserException += ParserExceptionEventHandler;
            _vStreamParser.PduError += PduErrorEventHandler;
            //Start stream parser
            _vStreamParser.Start();
        }

        private void DisassembleComponents()
        {
            if (_vStreamParser == null) { return; }
            _vStreamParser.ParserException -= ParserExceptionEventHandler;
            _vStreamParser.PduError -= PduErrorEventHandler;
            //Stop parser first
            _vStreamParser.Stop(true);
            _vStreamParser = null;
            _vTrans = null;
            _vRespHandler = null;
        }

        private void BindSession(SessionBindInfo bindInfo, int timeOut)
        {
            _vTcpIpSession.SessionClosed += TcpIpSessionClosedEventHandler;

            BindRequest bindReq = bindInfo.CreatePdu();
            _vTrans.Send(bindReq);
            BindResponse bindResp = null;
            try { bindResp = (BindResponse)_vRespHandler.WaitResponse(bindReq, timeOut); }
            catch (SmppResponseTimedOutException ex)
            { throw new SmppBindException(ex); }
            if (bindResp.Header.ErrorCode != 0)
            { throw new SmppBindException(bindResp.Header.ErrorCode); }
            //Copy settings
            _vSmscId = bindResp.SystemId;
            _vSystemId = bindInfo.SystemId;
            _vPassword = bindInfo.Password;
            _vAddressTon = bindInfo.AddressTon;
            _vAddressNpi = bindInfo.AddressNpi;
            //Start timer
            _vTimer.Start();
            _vIsAlive = true;
            switch (bindReq.Header.CommandType)
            {
                case CommandType.BindTransceiver:
                    ChangeState(SmppSessionState.Transceiver);
                    break;
                case CommandType.BindReceiver:
                    ChangeState(SmppSessionState.Receiver);
                    break;
                case CommandType.BindTransmitter:
                    ChangeState(SmppSessionState.Transmitter);
                    break;
            }
        }

        private void InitializeTimer()
        {
            _vTimer = new Timer(DefaultDelay);
            _vTimer.Elapsed += new ElapsedEventHandler(TimerCallback);
        }

        private void DestroyTimer()
        {
            try { _vTimer.Stop(); _vTimer.Close(); }
            catch {/*Silent catch*/}
        }

        private void ChangeState(SmppSessionState newState)
        {
            lock (_vSyncRoot)
            {
                _vState = newState;
            }
        }

        private bool CheckState(Pdu pdu)
        {
            return (int)(pdu.AllowedSession & _vState) != 0;
        }

        private void TimerCallback(object sender, ElapsedEventArgs e)
        {
            EnquireLink enqLink = new EnquireLink();
            //Send EnquireLink with 5 seconds response timeout
            try
            {
                EnquireLinkResp resp = (EnquireLinkResp)SendPdu(enqLink, 5000);
            }
            catch (SmppResponseTimedOutException)
            {
                //If there was no response, we conclude that this session is no longer active
                //Shutdown this session
                EndSession(SmppSessionCloseReason.EnquireLinkTimeout, null);
            }
            catch (SmppException) { /*Silent catch*/} //Incorrect bind status for a given command
            catch (TcpIpException) {/*Silent catch*/ }
        }

        private void PduRequestProcessorCallback(RequestPdu pdu)
        {
            ResponsePdu resp = null;
            if (pdu is Unbind)
            {
                resp = pdu.CreateDefaultResponce();
                try { SendPduBase(resp); }
                catch {/*silent catch*/}
                EndSession(SmppSessionCloseReason.UnbindRequested, null);
                return;
            }
            resp = RaisePduReceivedEvent(pdu);
            if (resp == null)
            {
                if (pdu.HasResponse)
                {
                    resp = pdu.CreateDefaultResponce();
                }
            }
            if (resp != null)
            { try { SendPduBase(resp); } catch {/*Silent catch*/} }
        }

        private void TcpIpSessionClosedEventHandler(object sender, TcpIpSessionClosedEventArgs e)
        {
            switch (e.CloseReason)
            {
                case SessionCloseReason.EndSessionCalled:
                    EndSession(SmppSessionCloseReason.TcpIpSessionClosed, e.Exception);
                    break;
                case SessionCloseReason.ExceptionThrown:
                    EndSession(SmppSessionCloseReason.TcpIpSessionError, e.Exception);
                    break;
            }
        }

        private void ParserExceptionEventHandler(object sender, ParserExceptionEventArgs e)
        {
            EndSession(SmppSessionCloseReason.TcpIpSessionError, e.Exception);
        }

        private void PduErrorEventHandler(object sender, PduErrorEventArgs e)
        {
            ResponsePdu resp = null;
            if (e.Pdu is RequestPdu)
            {
                RequestPdu req = (RequestPdu)e.Pdu;
                resp = req.CreateDefaultResponce();
                resp.Header.ErrorCode = e.Exception.ErrorCode;
            }
            else
            {
                resp = new GenericNack(e.Header);
                resp.Header.ErrorCode = e.Exception.ErrorCode;
            }
            try { SendPduBase(resp); }
            catch {/*silent catch*/}
        }

        private ResponsePdu RaisePduReceivedEvent(RequestPdu pdu)
        {
            /*
             * PduReceived event is not raised asynchronously as this method is 
             * being called asynchronously by a worker thread from the thread pool.
             */
            if (PduReceived == null) { return null; }
            PduReceivedEventArgs e = new PduReceivedEventArgs(pdu);
            PduReceived(this, e);
            return e.Response;
        }

        private void RaiseSessionClosedEvent(SmppSessionCloseReason reason, Exception exception)
        {
            if (SessionClosed == null) { return; }
            SmppSessionClosedEventArgs e = new SmppSessionClosedEventArgs(reason, exception);
            foreach (EventHandler<SmppSessionClosedEventArgs> del in SessionClosed.GetInvocationList())
            { del.BeginInvoke(this, e, AsyncCallBackRaiseSessionClosedEvent, del); }
        }

        private void AsyncCallBackRaiseSessionClosedEvent(IAsyncResult result)
        {
            EventHandler<SmppSessionClosedEventArgs> del = 
                (EventHandler<SmppSessionClosedEventArgs>) result.AsyncState;
            del.EndInvoke(result);
        }
        #endregion
        #endregion
    }
}
