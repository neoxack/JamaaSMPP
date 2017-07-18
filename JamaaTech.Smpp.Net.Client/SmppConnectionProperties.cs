/************************************************************************
 * Copyright (C) 2008 Jamaa Technologies
 *
 * This file is part of Jamaa SMPP Client Library.
 *
 * Jamaa SMPP Client Library is free software. You can redistribute it and/or modify
 * it under the terms of the Microsoft Reciprocal License (Ms-RL)
 *
 * You should have received a copy of the Microsoft Reciprocal License
 * along with Jamaa SMPP Client Library; See License.txt for more details.
 *
 * Author: Benedict J. Tesha
 * benedict.tesha@jamaatech.com, www.jamaatech.com
 *
 ************************************************************************/

using System;
using JamaaTech.Smpp.Net.Lib;

namespace Jamaa.Smpp.Net.Client
{
    /// <summary>
    /// Represents SMPP connection properties
    /// </summary>
    [Serializable()]
    public class SmppConnectionProperties
    {
        #region Variables
        private string _vSystemId;
        private string _vPassword;
        private string _vHost;
        private int _vPort;
        private InterfaceVersion _vInterfaceVersion;
        private TypeOfNumber _vAddressTon;
        private NumberingPlanIndicator _vAddressNpi;
        private DataCoding _vDefaultEncoding;
        private string _vDefaultServiceType;
        private string _vSystemType;
        private readonly object _vSyncRoot;
        private string _vSourceAddress;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="SmppConnectionProperties"/>
        /// </summary>
        public SmppConnectionProperties()
        {
            _vSystemId = "";
            _vPassword = "";
            _vHost = "";
            _vAddressTon = TypeOfNumber.International;
            _vAddressNpi = NumberingPlanIndicator.Isdn;
            _vInterfaceVersion = InterfaceVersion.V34;
            _vSystemType = "";
            _vDefaultServiceType = "";
            SmscId = "";
            _vSyncRoot = new object();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the system id that identifies this client to the SMPP server
        /// </summary>
        public string SystemId
        {
            get { return _vSystemId; }
            set { _vSystemId = value; }
        }

        /// <summary>
        /// Gets or sets the password for authenticating the client to the SMPP server
        /// </summary>
        public string Password
        {
            get { return _vPassword; }
            set { _vPassword = value; }
        }

        /// <summary>
        /// Gets or sets host name or IP address of the remote host
        /// </summary>
        public string Host
        {
            get { return _vHost; }
            set { _vHost = value; }
        }

        /// <summary>
        /// Gets or sets the TCP/IP Protocol port number
        /// </summary>
        public int Port
        {
            get { return _vPort; }
            set { _vPort = value; }
        }

        /// <summary>
        /// Gets or sets the default SMPP interface version to be used
        /// </summary>
        public InterfaceVersion InterfaceVersion
        {
            get { return _vInterfaceVersion; }
            set { _vInterfaceVersion = value; }
        }

        /// <summary>
        /// Gets or sets the Numbering Plan Indicator (NPI)
        /// </summary>
        public NumberingPlanIndicator AddressNpi
        {
            get { return _vAddressNpi; }
            set { _vAddressNpi = value; }
        }

        /// <summary>
        /// Gets or sets the type of number
        /// </summary>
        public TypeOfNumber AddressTon
        {
            get { return _vAddressTon; }
            set { _vAddressTon = value; }
        }

        /// <summary>
        /// Gets or sets the default encoding to be used when sending messages
        /// </summary>
        public DataCoding DefaultEncoding
        {
            get { return _vDefaultEncoding; }
            set { _vDefaultEncoding = value; }
        }

        /// <summary>
        /// Gets or sets the defalt SMPP service type
        /// </summary>
        public string DefaultServiceType
        {
            get { return _vDefaultServiceType; }
            set { _vDefaultServiceType = value; }
        }

        /// <summary>
        /// Gets or sets SMPP service type
        /// </summary>
        public string SystemType
        {
            get { return _vSystemType; }
            set { _vSystemType = value; }
        }

        /// <summary>
        /// Gets the ID or the Short Message Service Center (SMSC)
        /// </summary>
        public string SmscId { internal set; get; }

        /// <summary>
        /// Gets an object that can be used for locking in a multi-threaded environment
        /// </summary>
        public object SyncRoot => _vSyncRoot;

        /// <summary>
        /// Gets or sets the default source address when sending messages
        /// </summary>
        public string SourceAddress
        {
            get { return _vSourceAddress; }
            set { _vSourceAddress = value; }
        }
        #endregion

        #region Methods
        internal SessionBindInfo GetBindInfo()
        {
            SessionBindInfo bindInfo = new SessionBindInfo();
            bindInfo.SystemId = _vSystemId;
            bindInfo.Password = _vPassword;
            bindInfo.ServerName = _vHost;
            bindInfo.Port = _vPort;
            bindInfo.InterfaceVersion = _vInterfaceVersion;
            bindInfo.AddressTon = _vAddressTon;
            bindInfo.AddressNpi = _vAddressNpi;
            bindInfo.SystemType = _vSystemType;
            return bindInfo;
        }
        #endregion
    }
}
