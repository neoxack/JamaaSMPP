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

using JamaaTech.Smpp.Net.Lib.Util;

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public sealed class SmppAddress
    {
        #region Variables
        private TypeOfNumber _vTon;
        private NumberingPlanIndicator _vNpi;
        private string _vAddress;
        #endregion

        #region Constructors
        public SmppAddress()
        {
            _vTon = TypeOfNumber.International;
            _vNpi = NumberingPlanIndicator.Isdn;
            _vAddress = "";
        }

        public SmppAddress(TypeOfNumber ton, NumberingPlanIndicator npi, string address)
        {
            _vTon = ton;
            _vNpi = npi;
            _vAddress = address;
        }
        #endregion

        #region Properties
        public TypeOfNumber Ton
        {
            get { return _vTon; }
            set { _vTon = value; }
        }

        public NumberingPlanIndicator Npi
        {
            get { return _vNpi; }
            set { _vNpi = value; }
        }

        public string Address
        {
            get { return _vAddress; }
            set { _vAddress = value; }
        }
        #endregion

        #region Methods
        internal static SmppAddress Parse(ByteBuffer buffer)
        {
            //We require at least 3 bytes for SMPPAddress instance to be craeted
            if (buffer.Length < 3) { throw new NotEnoughBytesException("SMPPAddress requires at least 3 bytes"); }
            TypeOfNumber ton = (TypeOfNumber)Pdu.GetByte(buffer);
            NumberingPlanIndicator npi = (NumberingPlanIndicator)Pdu.GetByte(buffer);
            string address = Pdu.DecodeCString(buffer);
            return new SmppAddress(ton, npi, address);
        }

        public byte[] GetBytes()
        {
            //Approximate buffer required;
            int capacity = 4 + _vAddress == null ? 1 : _vAddress.Length;
            ByteBuffer buffer = new ByteBuffer(capacity);
            buffer.Append((byte)_vTon);
            buffer.Append((byte)_vNpi);
            buffer.Append(Pdu.EncodeCString(_vAddress));
            return buffer.ToBytes();
        }
        #endregion
    }
}
