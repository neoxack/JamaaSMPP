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

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public class Unbind : GenericRequestPdu
    {
        #region Constructors
        internal Unbind(PduHeader header)
            : base(header) { }

        public Unbind()
            : base(new PduHeader(CommandType.UnBind))
        {
        }
        #endregion

        #region Properties
        public override SmppEntityType AllowedSource => SmppEntityType.Any;

        public override SmppSessionState AllowedSession => SmppSessionState.Transceiver;

        #endregion

        #region Methods
        public override ResponsePdu CreateDefaultResponce()
        {
            PduHeader header = new PduHeader(CommandType.UnBindResp,VHeader.SequenceNumber);
            UnbindResp resp = (UnbindResp)CreatePdu(header);
            return resp;
        }
        #endregion

    }
}
