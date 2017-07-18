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

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public sealed class ReplaceSm : SmOperationPdu
    {
        #region Variables
        private string _vScheduleDeliveryTime;
        private string _vValidityPeriod;
        private RegisteredDelivery _vRegisteredDelivery;
        private byte _vSmDefaultMessageId;
        private byte _vSmLength;
        private string _vShortMessage;
        #endregion

        #region Constructors
        public ReplaceSm()
            : base(new PduHeader(CommandType.ReplaceSm))
        {
            _vScheduleDeliveryTime = "";
            _vValidityPeriod = "";
            _vRegisteredDelivery = RegisteredDelivery.None;
            _vSmDefaultMessageId = 0;
            _vShortMessage = "";
            _vSmLength = 0;
        }

        public ReplaceSm(PduHeader header)
            : base(header)
        {
             _vScheduleDeliveryTime = "";
            _vValidityPeriod = "";
            _vRegisteredDelivery = RegisteredDelivery.None;
            _vSmDefaultMessageId = 0;
            _vShortMessage = "";
            _vSmLength = 0;
       }
        #endregion

        #region Properties
        public override SmppEntityType AllowedSource => SmppEntityType.Esme;

        public override SmppSessionState AllowedSession => SmppSessionState.Transmitter;

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

        public RegisteredDelivery RegisteredDelivery
        {
            get { return _vRegisteredDelivery; }
            set { _vRegisteredDelivery = value; }
        }

        public byte SmDefaultMessageId
        {
            get { return _vSmDefaultMessageId; }
            set { _vSmDefaultMessageId = value; }
        }

        public byte SmLength => _vSmLength;

        public string ShortMessage
        {
            get { return _vShortMessage; }
            set { _vShortMessage = value; }
        }
        #endregion

        #region Methods
        public override ResponsePdu CreateDefaultResponce()
        {
            PduHeader header = new PduHeader(CommandType.ReplaceSm,VHeader.SequenceNumber);
            return new ReplaceSmResp(header);
        }

        protected override byte[] GetBodyData()
        {
            ByteBuffer buffer = new ByteBuffer(64);
            buffer.Append(EncodeCString(VMessageId));
            buffer.Append(VSourceAddress.GetBytes());
            buffer.Append(EncodeCString(_vScheduleDeliveryTime));
            buffer.Append(EncodeCString(_vValidityPeriod));
            buffer.Append((byte)_vRegisteredDelivery);
            buffer.Append((byte)_vSmDefaultMessageId);
            byte[] shortMessage = EncodeCString(_vShortMessage);
            _vSmLength = (byte)shortMessage.Length;
            buffer.Append((byte)_vSmLength);
            buffer.Append(shortMessage);
            return buffer.ToBytes();
       }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            VMessageId = DecodeCString(buffer);
            VSourceAddress = SmppAddress.Parse(buffer);
            _vScheduleDeliveryTime = DecodeCString(buffer);
            _vValidityPeriod = DecodeCString(buffer);
            _vRegisteredDelivery = (RegisteredDelivery)GetByte(buffer);
            _vSmDefaultMessageId = GetByte(buffer);
            _vSmLength = GetByte(buffer);
            _vShortMessage = DecodeString(buffer, (int)_vSmLength);
            //This pdu has no option parameters,
            //If there is something left in the buffer,
            //then we have more than required bytes
            if (buffer.Length > 0) { throw new TooManyBytesException(); }
        }
        #endregion
    }
}
