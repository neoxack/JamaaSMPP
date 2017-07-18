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

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public class PduException : Exception
    {
        #region Variables
        private readonly SmppErrorCode _vErrorCode;
        #endregion

        #region Constructors
        public PduException(SmppErrorCode errorCode) { _vErrorCode = errorCode; }

        public PduException(SmppErrorCode errorCode, string message)
            : base(message) { _vErrorCode = errorCode; }

        public PduException(SmppErrorCode errorCode, string message, Exception innerException)
            : base(message, innerException) { _vErrorCode = errorCode; }
        #endregion

        #region Properties
        public SmppErrorCode ErrorCode => _vErrorCode;

        #endregion
    }
}
