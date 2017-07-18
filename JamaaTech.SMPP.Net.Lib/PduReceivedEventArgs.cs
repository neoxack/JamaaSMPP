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
using JamaaTech.Smpp.Net.Lib.Protocol;

namespace JamaaTech.Smpp.Net.Lib
{
    public class PduReceivedEventArgs : EventArgs
    {
        #region Variables
        private readonly RequestPdu _vRequest;
        private ResponsePdu _vResponse;
        #endregion

        #region Constructors
        public PduReceivedEventArgs(RequestPdu request)
        {
            _vRequest = request;
        }
        #endregion

        #region Properties
        public RequestPdu Request => _vRequest;

        public ResponsePdu Response
        {
            get { return _vResponse; }
            set { _vResponse = value; }
        }
        #endregion
    }
}
