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

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public abstract class BindRequest : RequestPdu
    {
        #region Variables
        private string _vSystemId;
        private string _vPassword;
        private string _vSystemType;
        private TypeOfNumber _vAddressTon;
        private NumberingPlanIndicator _vAddressNpi;
        private byte _vInterfaceVersion;
        private string _vAddressRange;
        #endregion

        #region Constructors
        internal BindRequest(PduHeader header)
            : base(header) 
        {
            _vSystemId = "";
            _vPassword = "";
            _vSystemType = "";
            _vAddressTon = TypeOfNumber.International; //International
            _vAddressNpi = NumberingPlanIndicator.Isdn; //ISDN
            _vInterfaceVersion = 34; //SMPP 3.4 version
            _vAddressRange = "";
        }
        #endregion

        #region Properties

        public override SmppEntityType AllowedSource => SmppEntityType.Esme;

        public override SmppSessionState AllowedSession => SmppSessionState.Open;

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

        public TypeOfNumber AddressTon
        {
            get { return _vAddressTon; }
            set { _vAddressTon = value; }
        }

        public NumberingPlanIndicator AddressNpi
        {
            get { return _vAddressNpi; }
            set { _vAddressNpi = value; }
        }

        public byte InterfaceVersion
        {
            get { return _vInterfaceVersion; }
            set { _vInterfaceVersion = value; }
        }

        public string AddressRange
        {
            get { return _vAddressRange; }
            set { _vAddressRange = value; }
        }
        #endregion

        #region Methods
        #region Interface Methods
        public override ResponsePdu CreateDefaultResponce()
        {
            CommandType cmdType = CommandType.BindTransceiverResp;
            switch (VHeader.CommandType)
            {
                case CommandType.BindReceiver:
                    cmdType = CommandType.BindReceiverResp;
                    break;
                case CommandType.BindTransmitter:
                    cmdType = CommandType.BindTransmitterResp;
                    break;
            }
            PduHeader header = new PduHeader(cmdType, VHeader.SequenceNumber);
            return (BindResponse)CreatePdu(header);
        }

        protected override byte[] GetBodyData()
        {
            ByteBuffer buffer = new ByteBuffer(32);
            buffer.Append(EncodeCString(_vSystemId));
            buffer.Append(EncodeCString(_vPassword));
            buffer.Append(EncodeCString(_vSystemType));
            buffer.Append(_vInterfaceVersion);
            buffer.Append((byte)_vAddressTon);
            buffer.Append((byte)_vAddressNpi);
            buffer.Append(EncodeCString(_vAddressRange));
            return buffer.ToBytes();
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            const int minBytes = 7;
            if (buffer.Length < minBytes) { throw new NotEnoughBytesException("BindRequest requires at least 7 bytes for body parameters"); }
            try
            {
                _vSystemId = DecodeCString(buffer);
                _vPassword = DecodeCString(buffer);
                _vSystemType = DecodeCString(buffer);
                _vInterfaceVersion = GetByte(buffer);
                _vAddressTon = (TypeOfNumber)GetByte(buffer);
                _vAddressNpi = (NumberingPlanIndicator)GetByte(buffer);
                _vAddressRange = DecodeCString(buffer);
            }
            catch (InvalidOperationException ex)
            {
                //ByteBuffer.Remove() throws InvalidOperationException exception if called on a empty ByteBuffer instance
                //Wrap this exception as a NotEnoughBytesException exception
                throw new NotEnoughBytesException(ex.Message,ex);
            }
            if (buffer.Length > 0) //If there are some bytes left
            { throw new TooManyBytesException(); }
        }
        #endregion

        #region Helper Methods
        #endregion
        #endregion
    }
}
