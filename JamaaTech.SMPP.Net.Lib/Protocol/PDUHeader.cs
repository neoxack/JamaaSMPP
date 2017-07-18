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
    public sealed class PduHeader
    {
        #region Variables
        private readonly CommandType _vCommandType;
        private uint _vCommandLength;
        private SmppErrorCode _vErrorCode;
        private readonly uint _vSequenceNumber;

        private static uint _vNextSequenceNumber;
        private static readonly object _vSyncRoot;
        #endregion

        #region Constructors
        static PduHeader()
        {
            _vSyncRoot = new object();
            _vNextSequenceNumber = 1;
        }

        public PduHeader(CommandType cmdType)
            : this(cmdType, GetNextSequenceNumber()) 
        {
            _vCommandLength = 16;
        }

        public PduHeader(CommandType cmdType, uint seqNumber)
        {
            _vCommandType = cmdType;
            _vSequenceNumber = seqNumber;
            _vCommandLength = 16;
        }

        public PduHeader(CommandType cmdType, uint seqNumber, SmppErrorCode errorCode)
            :this(cmdType,seqNumber)
        {
            _vErrorCode = errorCode;
            _vCommandLength = 16;
        }

        public PduHeader(CommandType cmdType, uint seqNumber, SmppErrorCode errorCode, uint cmdLength)
            :this(cmdType,seqNumber,errorCode)
        {
            _vCommandLength = cmdLength;
        }
        #endregion

        #region Properties
        public CommandType CommandType => _vCommandType;

        public uint CommandLength
        {
            get { return _vCommandLength; }
            set { _vCommandLength = value; }
        }

        public SmppErrorCode ErrorCode
        {
            get { return _vErrorCode; }
            set { _vErrorCode = value; }
        }

        public uint SequenceNumber => _vSequenceNumber;

        #endregion

        #region Methods
        public static PduHeader Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            if (buffer.Length < 16) { throw new ArgumentException("Buffer length must not be less than 16 bytes"); }
            uint cmdLength = SmppEncodingUtil.GetIntFromBytes(buffer.Remove(4));
            CommandType cmdType = (CommandType)SmppEncodingUtil.GetIntFromBytes(buffer.Remove(4));
            SmppErrorCode errorCode = (SmppErrorCode)SmppEncodingUtil.GetIntFromBytes(buffer.Remove(4));
            uint seqNumber = SmppEncodingUtil.GetIntFromBytes(buffer.Remove(4));
            PduHeader header = new PduHeader(cmdType, seqNumber, errorCode, cmdLength);
            return header;
        }

        public byte[] GetBytes()
        {
            ByteBuffer buffer = new ByteBuffer(32);
            buffer.Append(SmppEncodingUtil.GetBytesFromInt(_vCommandLength));
            buffer.Append(SmppEncodingUtil.GetBytesFromInt((uint)_vCommandType));
            buffer.Append(SmppEncodingUtil.GetBytesFromInt((uint)_vErrorCode));
            buffer.Append(SmppEncodingUtil.GetBytesFromInt(_vSequenceNumber));
            return buffer.ToBytes();
        }

        public static uint GetNextSequenceNumber()
        {
            lock (_vSyncRoot)
            {
                uint seqNumber = _vNextSequenceNumber;
                if (_vNextSequenceNumber == uint.MaxValue) { _vNextSequenceNumber = 1; }
                else { _vNextSequenceNumber++; }
                return seqNumber;
            }
        }

        public override string ToString()
        {
            return _vCommandType.ToString();
        }
        #endregion
    }
}
