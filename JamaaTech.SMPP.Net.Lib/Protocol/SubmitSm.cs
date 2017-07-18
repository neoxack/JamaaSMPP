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
using JamaaTech.Smpp.Net.Lib.Util;
using JamaaTech.Smpp.Net.Lib.Protocol.Tlv;

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public class SubmitSm : SingleDestinationPdu
    {
        #region Variables
        private byte _vProtocolId;
        private PriorityFlag _vPriorityFlag;
        private string _vScheduleDeliveryTime;
        private string _vValidityPeriod;
        private bool _vReplaceIfPresent;
        private byte _vSmDefalutMessageId;
        private byte[] _vMessageBytes;
        #endregion

        #region Properties
        public override SmppEntityType AllowedSource => SmppEntityType.Esme;

        public override SmppSessionState AllowedSession => SmppSessionState.Transmitter;

        public byte ProtocolId
        {
            get { return _vProtocolId; }
            set { _vProtocolId = value; }
        }

        public PriorityFlag PriorityFlag
        {
            get { return _vPriorityFlag; }
            set { _vPriorityFlag = value; }
        }

        public string ScheduleDeliveryTime
        {
            get { return _vScheduleDeliveryTime; }
            set { _vScheduleDeliveryTime = value; }
        }

        public string ValidityPeriod
        {
            get { return _vValidityPeriod; }
            set { _vValidityPeriod = value; }
        }

        public bool ReplaceIfPresent
        {
            get { return _vReplaceIfPresent; }
            set { _vReplaceIfPresent = value; }
        }

        public byte SmDefaultMessageId
        {
            get { return _vSmDefalutMessageId; }
            set { _vSmDefalutMessageId = value; }
        }
        #endregion

        #region Constructors
        public SubmitSm(PduHeader header)
            : base(header)
        {
            VServiceType = Protocol.ServiceType.Default;
            _vProtocolId = 0;
            _vPriorityFlag = PriorityFlag.Level0;
            _vScheduleDeliveryTime = "";
            _vValidityPeriod = "";
            VRegisteredDelivery = RegisteredDelivery.None;
            _vReplaceIfPresent = false;
            VDataCoding = DataCoding.Ascii;
            _vSmDefalutMessageId = 0;
        }

        public SubmitSm()
            : this(new PduHeader(CommandType.SubmitSm))
        { }
        #endregion

        #region Methods
        public override ResponsePdu CreateDefaultResponce()
        {
            PduHeader header = new PduHeader(CommandType.DeliverSmResp, VHeader.SequenceNumber);
            return new SubmitSmResp(header);
        }

        protected override byte[] GetBodyData()
        {
            ByteBuffer buffer = new ByteBuffer(256);
            buffer.Append(EncodeCString(VServiceType));
            buffer.Append(VSourceAddress.GetBytes());
            buffer.Append(VDestinationAddress.GetBytes());
            buffer.Append((byte)VEsmClass);
            buffer.Append(_vProtocolId);
            buffer.Append((byte)_vPriorityFlag);
            buffer.Append(EncodeCString(_vScheduleDeliveryTime));
            buffer.Append(EncodeCString(_vValidityPeriod));
            buffer.Append((byte)VRegisteredDelivery);
            buffer.Append(_vReplaceIfPresent ? (byte)1 : (byte)0);
            buffer.Append((byte)VDataCoding);
            buffer.Append(_vSmDefalutMessageId);
            //Check if vMessageBytes is not null
            if (_vMessageBytes == null)
            {
                //Check whether optional field is used
                if (VTlv.GetTlvByTag(Tag.MessagePayload) == null)
                {
                    //Create an empty message
                    _vMessageBytes = new byte[] { 0x00 };
                }
            }
            if (_vMessageBytes == null) { buffer.Append(0); }
            else
            {
                buffer.Append((byte)_vMessageBytes.Length);
                buffer.Append(_vMessageBytes);
            }
            return buffer.ToBytes();
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            VServiceType = DecodeCString(buffer);
            VSourceAddress = SmppAddress.Parse(buffer);
            VDestinationAddress = SmppAddress.Parse(buffer);
            VEsmClass = (EsmClass)GetByte(buffer);
            _vProtocolId = GetByte(buffer);
            _vPriorityFlag = (PriorityFlag)GetByte(buffer);
            _vScheduleDeliveryTime = DecodeCString(buffer);
            _vValidityPeriod = DecodeCString(buffer);
            VRegisteredDelivery = (RegisteredDelivery)GetByte(buffer);
            _vReplaceIfPresent = GetByte(buffer) == 0 ? false : true;
            VDataCoding = (DataCoding)GetByte(buffer);
            _vSmDefalutMessageId = GetByte(buffer);
            int length = GetByte(buffer);
            if (length == 0) { _vMessageBytes = null; }
            else
            {
                if (length > buffer.Length)
                {
                    throw new NotEnoughBytesException("Pdu encoutered less bytes than indicated by message length");
                }
                _vMessageBytes = buffer.Remove(length);
            }
            if (buffer.Length > 0) { VTlv = TlvCollection.Parse(buffer); }
        }

        public override byte[] GetMessageBytes()
        {
            if (_vMessageBytes != null) { return _vMessageBytes; }
            //Otherwise, check if the 'message_payload' field is used
            Tlv.Tlv tlv = VTlv.GetTlvByTag(Tag.MessagePayload);
            if (tlv == null) { return null; }
            return tlv.RawValue;
        }

        public override  void SetMessageBytes(byte[] message)
        {
            if (message != null && message.Length > 254)
            { throw new ArgumentException("Message length cannot be greater than 254 bytes"); }
            _vMessageBytes = message;
        }
        #endregion
    }
}
