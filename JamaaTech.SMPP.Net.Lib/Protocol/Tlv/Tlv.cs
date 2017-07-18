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

namespace JamaaTech.Smpp.Net.Lib.Protocol.Tlv
{
    public class Tlv
    {
        #region Variables
        private readonly Tag _vTag;
        private ushort _vLength;
        private byte[] _vRawValue;
        #endregion

        #region Constructors
        public Tlv(Tag tag, ushort length)
        {
            _vTag = tag;
            _vLength = length;
        }

        public Tlv(Tag tag, ushort length, byte[] value)
            :this(tag,length)
        {
            ParseValue(value);
        }
        #endregion

        #region Properties
        public Tag Tag => _vTag;

        public ushort Length
        {
            get { return _vLength; }
            set { _vLength = value; }
        }

        public byte[] RawValue => _vRawValue;

        #endregion

        #region Methods
        public virtual byte[] GetBytes()
        {
            if (_vRawValue == null || _vRawValue.Length != _vLength) 
            { throw new TlvException("Tlv value length inconsistent with length field or has no data set"); }
            ByteBuffer buffer = new ByteBuffer(_vLength + 4); //Reserve enough capacity for tag, length and value fields
            buffer.Append(SmppEncodingUtil.GetBytesFromShort((ushort)_vTag));
            buffer.Append(SmppEncodingUtil.GetBytesFromShort(_vLength));
            buffer.Append(_vRawValue);
            return buffer.ToBytes();
        }

        public static Tlv Parse(ByteBuffer buffer)
        {
            //Buffer must have at least 4 bytes for tag and length plus at least one byte for the value field
            if (buffer.Length < 5) { throw new TlvException("Tlv required at least 5 bytes"); }
            Tag tag = (Tag)SmppEncodingUtil.GetShortFromBytes(buffer.Remove(2));
            ushort len = SmppEncodingUtil.GetShortFromBytes(buffer.Remove(2));
            Tlv tlv = new Tlv(tag, len);
            tlv.ParseValue(buffer, len);
            return tlv;
        }

        public virtual void ParseValue(ByteBuffer buffer, ushort length)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            if (buffer.Length < length) { throw new TlvException(); }
            byte[] bytes = buffer.Remove(length);
            _vRawValue = bytes;
            _vLength = length;
        }

        public virtual void ParseValue(byte[] bytes, int start, int length)
        {
            if (bytes == null) { throw new ArgumentNullException("bytes"); }
            if (length < 1) { throw new ArgumentException("Invalid length", "length"); }
            byte[] tempBytes = new byte[length];
            Array.Copy(bytes, start, tempBytes, 0, length);
            _vRawValue = tempBytes;
            _vLength = (ushort)length;
        }

        public void ParseValue(byte[] bytes)
        {
            if (bytes == null) { throw new ArgumentNullException("bytes"); }
            ParseValue(bytes, 0, bytes.Length);
        }
        #endregion
    }
}
