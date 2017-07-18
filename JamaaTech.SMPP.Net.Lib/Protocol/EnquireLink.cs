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
    public class EnquireLink : GenericRequestPdu
    {
        #region Constuctors
        internal EnquireLink(PduHeader header)
            : base(header) { }

        public EnquireLink()
            :base(new PduHeader(CommandType.EnquireLink))
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
            PduHeader header = new PduHeader(CommandType.EnquireLinkResp, VHeader.SequenceNumber);
            //use default Status and Length
            //header.CommandStatus = 0;
            //header.CommandLength = 16;
            EnquireLinkResp resp = (EnquireLinkResp)CreatePdu(header);
            return resp;
        }
        #endregion

    }
}
