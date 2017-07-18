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
    public sealed class QuerySmResp : ResponsePdu
    {
        #region Variables
        private string _vMessageId;
        private string _vFinalDate;
        private MessageState _vMessageState;
        private byte _vErrorCode;
        #endregion

        #region Constructors
        internal QuerySmResp(PduHeader header)
            : base(header)
        {
            _vMessageId = "";
            _vFinalDate = "";
            _vMessageState = MessageState.Unknown;
            _vErrorCode = 0;
        }
        #endregion

        #region Properties
        public override SmppEntityType AllowedSource => SmppEntityType.Smsc;

        public override SmppSessionState AllowedSession => SmppSessionState.Transmitter;

        public string MessageId
        {
            get { return _vMessageId; }
            set { _vMessageId = value; }
        }

        public string FinalDate
        {
            get { return _vFinalDate; }
            set { _vFinalDate = value; }
        }

        public MessageState MessageState
        {
            get { return _vMessageState; }
            set { _vMessageState = value; }
        }

        public byte ErrorCode
        {
            get { return _vErrorCode; }
            set { _vErrorCode = value; }
        }
        #endregion

        #region Methods
        protected override byte[] GetBodyData()
        {
            ByteBuffer buffer = new ByteBuffer(16);
            buffer.Append(EncodeCString(_vMessageId));
            buffer.Append(EncodeCString(_vFinalDate));
            buffer.Append((byte)_vMessageState);
            buffer.Append(_vErrorCode);
            return buffer.ToBytes();
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            _vMessageId = DecodeCString(buffer);
            _vFinalDate = DecodeCString(buffer);
            _vMessageState = (MessageState)GetByte(buffer);
            _vErrorCode = GetByte(buffer);
            //This pdu has no option parameters,
            //If the buffer still contains something,
            //the we received more that required bytes
            if (buffer.Length > 0) { throw new TooManyBytesException(); }
        }
        #endregion
    }
}
