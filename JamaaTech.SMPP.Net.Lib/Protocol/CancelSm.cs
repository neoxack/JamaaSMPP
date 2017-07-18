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
    public sealed class CancelSm : SmOperationPdu
    {
        #region Variables
        private SmppAddress _vDestinationAddress;
        #endregion

        #region Constuctors
        public CancelSm()
            : base(new PduHeader(CommandType.CancelSm))
        {
            _vDestinationAddress = new SmppAddress();
        }

        internal CancelSm(PduHeader header)
            : base(header)
        {
            _vDestinationAddress = new SmppAddress();
        }
        #endregion

        #region Properties
        public override SmppEntityType AllowedSource => SmppEntityType.Esme;

        public override SmppSessionState AllowedSession => SmppSessionState.Transmitter;

        public SmppAddress DestionationAddress => _vDestinationAddress;

        #endregion

        #region Methods
        public override ResponsePdu CreateDefaultResponce()
        {
            PduHeader header = new PduHeader(CommandType.CancelSmResp, VHeader.SequenceNumber);
            return new CancelSmResp(header);
        }

        protected override byte[] GetBodyData()
        {
            ByteBuffer buffer = new ByteBuffer(64);
            buffer.Append(EncodeCString(VMessageId));
            buffer.Append(VSourceAddress.GetBytes());
            buffer.Append(_vDestinationAddress.GetBytes());
            return buffer.ToBytes();
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            VMessageId = DecodeCString(buffer);
            VSourceAddress = SmppAddress.Parse(buffer);
            _vDestinationAddress = SmppAddress.Parse(buffer);
            //If this pdu has no option parameters
            //If there is still something in the buffer, we then have more than required bytes
            if (buffer.Length > 0) { throw new TooManyBytesException(); }
        }
        #endregion
    }
}
