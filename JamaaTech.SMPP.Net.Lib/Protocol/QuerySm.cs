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
    public sealed class QuerySm : SmOperationPdu
    {
        #region Constructors
        public QuerySm()
            : base(new PduHeader(CommandType.QuerySm)) { }

        internal QuerySm(PduHeader header)
            : base(header) { }
        #endregion

        #region Properties
        public override SmppEntityType AllowedSource => SmppEntityType.Esme;

        public override SmppSessionState AllowedSession => SmppSessionState.Transmitter;

        #endregion

        #region Methods
        public override ResponsePdu CreateDefaultResponce()
        {
            PduHeader header = new PduHeader(CommandType.QuerySmResp, VHeader.SequenceNumber);
            return new QuerySmResp(header);
        }

        protected override byte[] GetBodyData()
        {
            ByteBuffer buffer = new ByteBuffer(16);
            buffer.Append(EncodeCString(VMessageId));
            buffer.Append(VSourceAddress.GetBytes());
            return buffer.ToBytes();
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            VMessageId = DecodeCString(buffer);
            VSourceAddress = SmppAddress.Parse(buffer);
            //This pdu has no option parameters
            //If there is still something in the buffer,
            //we then have more than required bytes
            if (buffer.Length > 0) { throw new TooManyBytesException(); }
        }
        #endregion
    }
}
