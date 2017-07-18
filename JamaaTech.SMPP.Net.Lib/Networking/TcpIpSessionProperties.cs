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

using System.Net;
using System.Net.Sockets;

namespace JamaaTech.Smpp.Net.Lib.Networking
{
    public class TcpIpSessionProperties
    {
        #region Variables
        private readonly Socket _vSocket;
        #endregion

        #region Constructors
        internal TcpIpSessionProperties(Socket socket)
        {
            _vSocket = socket;
        }
        #endregion

        #region Properties
        public LingerOption LingerState
        {
            get { return _vSocket.LingerState; }
            set { _vSocket.LingerState = value; }
        }

        public bool NoDelay
        {
            get { return _vSocket.NoDelay; }
            set { _vSocket.NoDelay = value; }
        }

        public int ReceiveTimeout
        {
            get { return _vSocket.ReceiveTimeout; }
            set { _vSocket.ReceiveTimeout = value; }
        }

        public int ReceiveBufferSize
        {
            get { return _vSocket.ReceiveBufferSize; }
            set { _vSocket.ReceiveBufferSize = value; }
        }

        public int SendTimeout
        {
            get { return _vSocket.SendTimeout; }
            set { _vSocket.SendTimeout = value; }
        }

        public int SendBufferSize
        {
            get { return _vSocket.SendBufferSize; }
            set { _vSocket.SendBufferSize = value; }
        }

        public IPEndPoint LocalEndPoint => (IPEndPoint)_vSocket.LocalEndPoint;

        public IPEndPoint RemoteEndPoint => (IPEndPoint)_vSocket.RemoteEndPoint;

        #endregion
    }
}
