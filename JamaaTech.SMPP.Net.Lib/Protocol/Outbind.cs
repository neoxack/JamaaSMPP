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
    public class Outbind : RequestPdu
    {
        #region Variables
        private string _vSystemId;
        private string _vPassword;
        #endregion

        #region Constructors
        internal Outbind(PduHeader header)
            : base(header)
        {
            _vSystemId = "";
            _vPassword = "";
        }
        #endregion

        #region Properties

        public override SmppEntityType AllowedSource => SmppEntityType.Smsc;

        public override SmppSessionState AllowedSession => SmppSessionState.Open;

        public string SystemId
        {
            get { return _vSystemId; }
            set { _vSystemId = value; }
        }

        public string Password
        {
            get { return _vPassword; }
            set { _vPassword = value; }
        }
        #endregion

        #region Methods
        public override ResponsePdu CreateDefaultResponce()
        {
            PduHeader header = new PduHeader(CommandType.BindTransceiver, VHeader.SequenceNumber);
            return new BindTransceiverResp(header);
        }

        protected override byte[] GetBodyData()
        {
            ByteBuffer buffer = new ByteBuffer(_vSystemId.Length + _vPassword.Length + 2);
            buffer.Append(EncodeCString(_vSystemId));
            buffer.Equals(EncodeCString(_vPassword));
            return buffer.ToBytes();
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            //Outbind PDU requires at least 2 bytes
            if (buffer.Length < 2) { throw new NotEnoughBytesException("Outbind PDU requires at least 2 bytes for body data"); }
            _vSystemId = DecodeCString(buffer);
            _vPassword = DecodeCString(buffer);
            //This PDU has no optional parameters
            //If we still have something in the buffer, we are having more bytes than we expected
            if (buffer.Length > 0) { throw new TooManyBytesException(); }
        }
        #endregion
    }
}
