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
    [Serializable()]
    public class SessionBindInfo
    {
        #region Variables
        private string _vHost;
        private int _vPort;
        private string _vSystemId;
        private string _vPassword;
        private string _vSystemType;
        private InterfaceVersion _vInterfaceVersion;
        private bool _vAllowReceive;
        private bool _vAllowTransmit;
        private TypeOfNumber _vAddressTon;
        private NumberingPlanIndicator _vAddressNpi;
        #endregion

        #region Constructors
        public SessionBindInfo()
        {
            _vHost = "";
            _vSystemId = "";
            _vPassword = "";
            _vSystemType = "";
            _vInterfaceVersion = InterfaceVersion.V34;
            _vAllowReceive = true;
            _vAllowTransmit = true;
            _vAddressTon = TypeOfNumber.International;
            _vAddressNpi = NumberingPlanIndicator.Isdn;
        }

        public SessionBindInfo(string systemId, string password)
            :this()
        {
            _vSystemId = systemId;
            _vPassword = password;
        }
        #endregion

        #region Properties
        public string ServerName
        {
            get { return _vHost; }
            set { _vHost = value; }
        }

        public int Port
        {
            get { return _vPort; }
            set { _vPort = value; }
        }

        public InterfaceVersion InterfaceVersion
        {
            get { return _vInterfaceVersion; }
            set { _vInterfaceVersion = value; }
        }

        public string SystemId
        {
            get { return _vSystemId; }
            set { _vSystemId = value; }
        }

        public string Password
        {
            get { return _vPassword; }
            set { _vPassword = value; }
        }

        public string SystemType
        {
            get { return _vSystemType; }
            set { _vSystemType = value; }
        }

        public NumberingPlanIndicator AddressNpi
        {
            get { return _vAddressNpi; }
            set { _vAddressNpi = value; }
        }

        public TypeOfNumber AddressTon
        {
            get { return _vAddressTon; }
            set { _vAddressTon = value; }
        }

        public bool AllowReceive
        {
            get { return _vAllowReceive; }
            set { _vAllowReceive = value; }
        }

        public bool AllowTransmit
        {
            get { return _vAllowTransmit; }
            set { _vAllowTransmit = value; }
        }
        #endregion

        #region Methods
        internal BindRequest CreatePdu()
        {
            BindRequest req = CreateBindBdu();
            req.AddressNpi = _vAddressNpi;
            req.AddressTon = _vAddressTon;
            req.SystemId = _vSystemId;
            req.Password = _vPassword;
            req.SystemType = _vSystemType;
            req.InterfaceVersion = _vInterfaceVersion == InterfaceVersion.V33 ? (byte)0x33 : (byte)0x34;
            return req;
        }

        private BindRequest CreateBindBdu()
        {
            if (_vAllowReceive && _vAllowTransmit) { return new BindTransceiver(); }
            else if (_vAllowTransmit) { return new BindTransmitter(); }
            else if (_vAllowReceive) { return new BindReceiver(); }
            else { throw new InvalidOperationException("Both AllowTransmit and AllowReceive cannot be set to false"); }
        }
        #endregion
    }
}
