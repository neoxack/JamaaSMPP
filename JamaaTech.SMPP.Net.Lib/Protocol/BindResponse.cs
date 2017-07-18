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
    public class BindResponse : ResponsePdu
    {
        #region Variables
        private string _vSystemId;
        #endregion

        #region Constructors
        internal BindResponse(PduHeader header)
            : base(header)
        {
            _vSystemId = "";
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

        #endregion

        #region Methods
        protected override byte[] GetBodyData()
        {
            return EncodeCString(_vSystemId);
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            //If Error code is not zero, buffer.Length may be zero
            //This may happen because the SMSC may not return the system_id field
            //if the origianl bind request contained an error.
            if (Header.ErrorCode != SmppErrorCode.EsmeRok && buffer.Length == 0) { _vSystemId = ""; return; }
            //Otherwise, there must be something in the buffer
            _vSystemId = DecodeCString(buffer);
            if (buffer.Length > 0) { VTlv = TlvCollection.Parse(buffer); }
        }
        #endregion
    }
}
