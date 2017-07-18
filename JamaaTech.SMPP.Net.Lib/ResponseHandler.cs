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

using System.Collections.Generic;
using JamaaTech.Smpp.Net.Lib.Protocol;
using System.Threading;

namespace JamaaTech.Smpp.Net.Lib
{
    public class ResponseHandler
    {
        #region Variables
        private int _vDefaultResponseTimeout;
        private readonly List<ResponsePdu> _vResponseQueue;
        private readonly List<PduWaitContext> _vWaitingQueue;
        private readonly AutoResetEvent _vResponseEvent;
        private readonly AutoResetEvent _vWaitingEvent;
        #endregion

        #region Constructors
        public ResponseHandler()
        {
            _vDefaultResponseTimeout = 5000; //Five seconds
            _vWaitingQueue = new List<PduWaitContext>(32);
            _vResponseQueue = new List<ResponsePdu>(32);
            _vResponseEvent = new AutoResetEvent(true);
            _vWaitingEvent = new AutoResetEvent(true);
        }
        #endregion

        #region Properties
        public int DefaultResponseTimeout
        {
            get { return _vDefaultResponseTimeout; }
            set
            {
                int timeOut = 5000;
                if (value > timeOut) { timeOut = value; }
                Interlocked.Exchange(ref _vDefaultResponseTimeout, timeOut);
            }
        }
        public int Count
        {
            get { lock (_vResponseQueue) { return _vResponseQueue.Count; } }
        }
        #endregion

        #region Methods
        #region Interface Methods
        public void Handle(ResponsePdu pdu)
        {
            AddResponse(pdu);
            _vWaitingEvent.WaitOne();
            try
            {
                uint sequenceNumber = pdu.Header.SequenceNumber;
                for (int index = 0; index < _vWaitingQueue.Count; ++index)
                {
                    PduWaitContext waitContext = _vWaitingQueue[index];
                    if (waitContext.SequenceNumber == sequenceNumber)
                    {
                        _vWaitingQueue.RemoveAt(index);
                        waitContext.AlertResponseReceived();
                        if (waitContext.TimedOut) { FetchResponse(sequenceNumber); }
                        return;
                    }
                }
            }
            finally { _vWaitingEvent.Set(); }
        }

        public ResponsePdu WaitResponse(RequestPdu pdu)
        {
            return WaitResponse(pdu, _vDefaultResponseTimeout);
        }

        public ResponsePdu WaitResponse(RequestPdu pdu, int timeOut)
        {
            uint sequenceNumber = pdu.Header.SequenceNumber;
            ResponsePdu resp = FetchResponse(sequenceNumber);
            if (resp != null) { return resp; }
            if (timeOut < 5000) { timeOut = _vDefaultResponseTimeout; }
            PduWaitContext waitContext = new PduWaitContext(sequenceNumber, timeOut);
            _vWaitingEvent.WaitOne();
            try { _vWaitingQueue.Add(waitContext); }
            finally { _vWaitingEvent.Set(); }
            waitContext.WaitForAlert();
            resp = FetchResponse(sequenceNumber);
            if (resp == null) { throw new SmppResponseTimedOutException(); }
            return resp;
        }
        #endregion

        #region Helper Methods
        private void AddResponse(ResponsePdu pdu)
        {
            _vResponseEvent.WaitOne();
            try { _vResponseQueue.Add(pdu); }
            finally { _vResponseEvent.Set(); }
        }

        private ResponsePdu FetchResponse(uint sequenceNumber)
        {
            _vResponseEvent.WaitOne();
            try 
            {
                for (int index = 0; index < _vResponseQueue.Count; ++index)
                {
                    ResponsePdu pdu = _vResponseQueue[index];
                    if (pdu.Header.SequenceNumber == sequenceNumber)
                    {
                        _vResponseQueue.RemoveAt(index);
                        return pdu;
                    }
                }
                return null;
            }
            finally { _vResponseEvent.Set(); }
        }
        #endregion
        #endregion
    }
}
