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
    public class PduErrorEventArgs : EventArgs
    {
        #region Variable
        private readonly PduException _vException;
        private readonly byte[] _vByteDump;
        private readonly PduHeader _vHeader;
        private readonly Pdu _vPdu;
        #endregion

        #region Constructors
        public PduErrorEventArgs(PduException exception, byte[] byteDump, PduHeader header)
        {
            _vException = exception;
            _vByteDump = byteDump;
            _vHeader = header;
        }

        public PduErrorEventArgs(PduException exception, byte[] byteDump, PduHeader header, Pdu pdu)
            :this(exception,byteDump,header)
        {
            _vPdu = pdu;
        }
        #endregion

        #region Properties
        public PduException Exception => _vException;

        public byte[] ByteDump => _vByteDump;

        public PduHeader Header => _vHeader;

        public Pdu Pdu => _vPdu;

        #endregion
    }
}
