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
    public sealed class DataSmResp : ResponsePdu
    {
        #region Variables
        private string _vMessageId;
        #endregion

        #region Constructors
        internal DataSmResp(PduHeader header)
            : base(header)
        {
            _vMessageId = "";
        }
        #endregion

        #region Properties
        public string MessageId
        {
            get { return _vMessageId; }
            set { _vMessageId = value; }
        }

        public override SmppEntityType AllowedSource => SmppEntityType.Any;

        public override SmppSessionState AllowedSession => SmppSessionState.Transceiver;

        #endregion

        #region Methods
        protected override byte[] GetBodyData()
        {
            return EncodeCString(_vMessageId);
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            //We require at least 1 byte for this pdu
            if (buffer.Length < 1) { throw new NotEnoughBytesException("data_sm_resp requires at least 1 byte of body data"); }
            _vMessageId = DecodeCString(buffer);
            if (buffer.Length > 0) { VTlv = TlvCollection.Parse(buffer); }
        }
        #endregion
    }
}
