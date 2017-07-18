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
    public class AlertNotification : SmPdu
    {
        #region Variables
        private SmppAddress _vEsmeAddress;
        #endregion

        #region Constructors
        public AlertNotification()
            : base(new PduHeader(CommandType.AlertNotification))
        {
            _vEsmeAddress = new SmppAddress();
        }

        internal AlertNotification(PduHeader header)
            : base(header)
        {
            _vEsmeAddress = new SmppAddress();
        }
        #endregion

        #region Properties
        public override bool HasResponse => false;

        public override SmppEntityType AllowedSource => SmppEntityType.Smsc;

        public override SmppSessionState AllowedSession => SmppSessionState.Receiver;

        public SmppAddress EsmeAddress => _vEsmeAddress;

        #endregion

        #region Methods
        public override ResponsePdu CreateDefaultResponce()
        {
            return null;
        }

        protected override byte[] GetBodyData()
        {
            byte[] sourceAddrBytes = VSourceAddress.GetBytes();
            byte[] esmeAddresBytes = _vEsmeAddress.GetBytes();
            ByteBuffer buffer = new ByteBuffer(sourceAddrBytes.Length + esmeAddresBytes.Length);
            buffer.Append(sourceAddrBytes);
            buffer.Append(esmeAddresBytes);
            return buffer.ToBytes();
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            VSourceAddress = SmppAddress.Parse(buffer);
            _vEsmeAddress = SmppAddress.Parse(buffer);
            //If there are some bytes left,
            //construct a tlv collection
            if (buffer.Length > 0) { VTlv = TlvCollection.Parse(buffer); }
        }
        #endregion
    }
}
