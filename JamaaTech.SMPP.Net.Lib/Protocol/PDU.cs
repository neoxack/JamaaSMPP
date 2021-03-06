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
using JamaaTech.Smpp.Net.Lib.Protocol.Tlv;
using JamaaTech.Smpp.Net.Lib.Util;

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public abstract class Pdu
    {
        #region Varibles
        protected PduHeader VHeader;
        protected TlvCollection VTlv;
        #endregion

        #region Constructors
        internal Pdu(PduHeader header)
        {
            VHeader = header;
            VTlv = new TlvCollection();
        }
        #endregion

        #region Properties
        public PduHeader Header => VHeader;

        public TlvCollection Tlv => VTlv;

        public abstract SmppEntityType AllowedSource { get;}

        public abstract SmppSessionState AllowedSession { get;}

        #endregion

        #region Methods
        #region Interface Methods
        public static GenericNack GenericNack(PduHeader header, SmppErrorCode errorCode)
        {
            if (header == null) { throw new ArgumentNullException("header"); }
            GenericNack gNack = (GenericNack)CreatePdu(header);
            gNack.Header.ErrorCode = errorCode;
            return gNack;
        }

        public virtual GenericNack GenericNack(SmppErrorCode errorCode)
        {
            PduHeader header = new PduHeader(CommandType.GenericNack,VHeader.SequenceNumber);
            header.ErrorCode = errorCode;
            GenericNack gNack = (GenericNack)CreatePdu(header);
            return gNack;
        }

        public virtual byte[] GetBytes()
        {
            byte[] bodyData = GetBodyData();
            byte[] tlvData = VTlv.GetBytes();
            int length = 16;
            length += bodyData == null ? 0 : bodyData.Length;
            length += tlvData == null ? 0 : tlvData.Length;
            VHeader.CommandLength = (uint)length;
            ByteBuffer buffer = new ByteBuffer(length); //Allocate buffer with enough capacity
            buffer.Append(VHeader.GetBytes());
            if (bodyData != null) { buffer.Append(bodyData); }
            if (tlvData != null) { buffer.Append(tlvData); }
            return buffer.ToBytes();
        }

        protected abstract byte[] GetBodyData();

        protected abstract void Parse(ByteBuffer buffer);

        public void SetBodyData(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            try { Parse(buffer); }
            catch (PduException) { throw; }
            catch (Exception ex) { PduParseException.WrapAndThrow(ex); }
        }

        public static Pdu CreatePdu(PduHeader header)
        {
            if (header == null) { throw new ArgumentNullException("header"); }
            switch (header.CommandType)
            {
                case CommandType.BindReceiver:
                    return new BindReceiver(header);
                    //--
                case CommandType.BindTransceiver:
                    return new BindTransceiver(header);
                    //--
                case CommandType.BindTransmitter:
                    return new BindTransmitter(header);
                    //--
                case CommandType.BindTransmitterResp:
                    return new BindTransmitterResp(header);
                    //--
                case CommandType.BindTransceiverResp:
                    return new BindTransceiverResp(header);
                    //--
                case CommandType.BindReceiverResp:
                    return new BindReceiverResp(header);
                    //--
                case CommandType.OutBind:
                    return new Outbind(header);
                    //--
                case CommandType.EnquireLink:
                    return new EnquireLink(header);
                    //--
                case CommandType.EnquireLinkResp:
                    return new EnquireLinkResp(header);
                    //--
                case CommandType.UnBind:
                    return new Unbind(header);
                    //--
                case CommandType.UnBindResp:
                    return new UnbindResp(header);
                    //--
                case CommandType.GenericNack:
                    return new GenericNack(header);
                    //--
                case CommandType.SubmitSm:
                    return new SubmitSm(header);
                    //--
                case CommandType.SubmitSmResp:
                    return new SubmitSmResp(header);
                    //--
                case CommandType.DataSm:
                    return new DataSm(header);
                    //--
                case CommandType.DataSmResp:
                    return new DataSmResp(header);
                    //--
                case CommandType.DeliverSm:
                    return new DeliverSm(header);
                    //--
                case CommandType.DeliverSmResp:
                    return new DeliverSmResp(header);
                    //--
                case CommandType.CancelSm:
                    return new CancelSm(header);
                    //--
                case CommandType.CancelSmResp:
                    return new CancelSmResp(header);
                    //--
                case CommandType.ReplaceSm:
                    return new ReplaceSm(header);
                    //--
                case CommandType.ReplaceSmResp:
                    return new ReplaceSmResp(header);
                    //--
                case CommandType.QuerySm:
                    return new QuerySm(header);
                    //--
                case CommandType.QuerySmResp:
                    return new QuerySmResp(header);
                    //--
                default:
                    throw new InvalidPduCommandException();
            }
        }
        #endregion

        #region Helper Methods
        internal static string DecodeCString(ByteBuffer buffer)
        {
            //Get next terminating null value
            int pos = buffer.Find(0x00);
            if (pos < 0) { throw new PduFormatException("CString type field could not be read. The terminating charactor is missing"); }
            try { string value = SmppEncodingUtil.GetCStringFromBytes(buffer.Remove(pos + 1)); return value; }
            catch (ArgumentException ex)
            {
                //ByteBuffer.Remove(int count) throw ArgumentException if the buffer length is less than count
                //This is the indication that the amount of bytes required could not be met
                //We wrap this exception as NotEnoughtBytesException exception
                throw new NotEnoughBytesException("PDU requires more bytes than supplied", ex);
            }
        }

        internal static byte[] EncodeCString(string str)
        {
            if (str == null) { str = ""; }
            return SmppEncodingUtil.GetBytesFromCString(str);
        }

        internal static string DecodeString(ByteBuffer buffer, int length)
        {
            try { string value = SmppEncodingUtil.GetStringFromBytes(buffer.Remove(length)); return value; }
            catch(ArgumentException ex)
            {
                //ByteBuffer.Remove(int count) throw ArgumentException if the buffer length is less than count
                //This is the indication that the amount of bytes required could not be met
                //We wrap this exception as NotEnoughtBytesException exception
                throw new NotEnoughBytesException(
                    "Octet String field could not be decoded because no enough bytes are evailable in the buffer",
                    ex);
            }
        }

        internal static byte GetByte(ByteBuffer buffer)
        {
            try { return buffer.Remove(); }
            catch (InvalidOperationException)
            {
                //ByteBuffer.Remove() throws invalid operation exception if the buffer is empty
                //We rethrow this error as a NotEnoughBytesException exception
                throw new NotEnoughBytesException("A byte field could not be read because the buffer is empty");
            }
        }

        internal static byte[] EncodeString(string str)
        {
            if (str == null) { str = ""; }
            return SmppEncodingUtil.GetBytesFromString(str);
        }
        #endregion

        #region Overriden System.Object Members
        public override string ToString()
        {
            return VHeader.ToString();
        }
        #endregion
        #endregion
    }
}
