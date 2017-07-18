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
    public abstract class SmOperationPdu : SmPdu
    {
        #region Variables
        protected string VMessageId;
        #endregion

        #region Constructors
        internal SmOperationPdu(PduHeader header)
            : base(header) 
        {
            VMessageId = "";
        }
        #endregion

        #region Properties
        public string MessageId
        {
            get { return VMessageId; }
            set { VMessageId = value; }
        }
        #endregion
    }
}
