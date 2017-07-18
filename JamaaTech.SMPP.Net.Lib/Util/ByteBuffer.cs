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
using System.Text;

namespace JamaaTech.Smpp.Net.Lib.Util
{
    public sealed class ByteBuffer
    {
        #region Variables
        private byte[] _vArrayBuffer;
        private int _vNextPosition;
        private int _vLength;
        private int _vCapacity;
        #endregion

        #region Constants
        private const int DefaultCapacity = 64;
        private const int MinCapacity = 16;
        #endregion

        #region Constructors
        public ByteBuffer()
        {
            //Create array buffer with default capacity
            _vArrayBuffer = new byte[DefaultCapacity];
            _vCapacity = DefaultCapacity;
        }

        public ByteBuffer(int capacity)
        {
            //Create array buffer with specified capacity
            //If capacity is less than 32, use default size
            if (capacity < MinCapacity) { capacity = DefaultCapacity; }
            _vArrayBuffer = new byte[capacity];
            _vCapacity = capacity;
        }

        public ByteBuffer(byte[] array)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            //Create array buffer with capacity equal to the array size
            //Or default capacity if the array size is less than 32
            _vCapacity = array.Length < MinCapacity ? DefaultCapacity : array.Length;
            _vArrayBuffer = new byte[_vCapacity];
            Append(array);
        }

        public ByteBuffer(byte[] array, int capacity)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            //Create array buffer with capacity equal to the specified size
            //If capacity is less than the array size or 32, use default size
            if (capacity < array.Length) { capacity = array.Length; }
            _vCapacity = capacity < MinCapacity ? DefaultCapacity : capacity;
            _vArrayBuffer = new byte[_vCapacity];
            Append(array);
        }

        public string DumpString()
        {
            StringBuilder builder = new StringBuilder();
            bool appendSpace = false;
            foreach (byte @byte in ToBytes())
            {
                if (appendSpace) { builder.Append(" "); }
                else { appendSpace = true; }
                builder.AppendFormat("{0:x}", @byte);
            }
            return builder.ToString();
        }
        #endregion

        #region Properties
        public int Capacity
        {
            get { return _vCapacity; }
            set { _vCapacity = value; }
        }

        public int Length => _vLength;

        private int Reserved => _vArrayBuffer.Length - _vNextPosition;

        #endregion

        #region Methods
        #region Public Methods
        public void Append(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            if (buffer.Length <= 0) { return; }
            Append(buffer._vArrayBuffer, buffer._vNextPosition - buffer._vLength, buffer._vLength);
        }

        public void Append(byte[] bytes)
        {
            if (bytes == null) { throw new ArgumentNullException("bytes"); }
            Append(bytes, 0, bytes.Length);
        }

        public void Append(byte[] bytes, int start, int length)
        {
            if (bytes == null) { throw new ArgumentNullException("bytes"); }
            if (Reserved < length) { RealocateBuffer(length); }
            Array.Copy(bytes, start, _vArrayBuffer, _vNextPosition, length);
            _vNextPosition += length;
            _vLength += length;
        }

        public void Append(byte value)
        {
            if (Reserved < 1) { RealocateBuffer(1); }
            _vArrayBuffer[_vNextPosition] = value;
            _vNextPosition++;
            _vLength++;
        }

        public byte[] Peek(int count)
        {
            if (count > _vLength) { throw new ArgumentException("count cannot be greater than buffer length", "count"); }
            if (count <= 0) { throw new ArgumentException("count must be less greater than or zero"); }
            byte[] result = new byte[count];
            Array.Copy(_vArrayBuffer, _vNextPosition - _vLength, result, 0, count);
            return result;
        }

        public byte[] Remove(int count)
        {
            byte[] result = Peek(count);
            _vLength -= count;
            return result;
        }

        public byte Remove()
        {
            if (_vLength < 1) { throw new InvalidOperationException("Array must must not be emty"); }
            byte result = _vArrayBuffer[_vNextPosition - _vLength];
            _vLength--;
            return result;
        }

        public void Clear()
        {
            _vNextPosition = 0;
            _vLength = 0;
        }

        public ByteBuffer Clone()
        {
            byte[] newBuffer = new byte[_vArrayBuffer.Length];
            if (Length > 0)
            {
                //Copy all bytes onto the new array
                Array.Copy(_vArrayBuffer, _vNextPosition - _vLength, newBuffer, 0, _vLength);
            }
            return new ByteBuffer(newBuffer, _vCapacity);
        }

        public byte[] ToBytes()
        {
            if (Length <= 0) { return new byte[0]; }
            return Peek(Length);
        }

        public int Find(byte value)
        {
            return Find(value, 0, Length - 1);
        }

        public int Find(byte value, int startIndex, int endIndex)
        {
            if (Length <= 0) { return -1; }
            if (startIndex >= Length || startIndex < 0) { throw new ArgumentOutOfRangeException("startIndex"); }
            if (endIndex < startIndex) { throw new ArgumentException("startIndex cannot be greater than endIndex"); }
            if (endIndex >= Length) { throw new ArgumentOutOfRangeException("endIndex"); }
            startIndex = startIndex + _vNextPosition - _vLength;
            endIndex = endIndex + _vNextPosition - _vLength;
            for (; startIndex <= endIndex; startIndex++)
            {
                if (_vArrayBuffer[startIndex] == value)
                { return startIndex + _vLength - _vNextPosition; }
            }
            return -1;
        }
        #endregion

        #region Helper Methods
        private void RealocateBuffer(int increment)
        {
            int newBufferSize = ((int)(increment / _vCapacity) + 1) * _vCapacity;
            //Hold the current array buffer
            byte[] currentBuffer = _vArrayBuffer;
            //Allocate a new array buffer
            _vArrayBuffer = new byte[_vArrayBuffer.Length + newBufferSize];
            int startIndex = _vNextPosition - _vLength;
            int length = _vLength;
            _vNextPosition = 0;
            _vLength = 0;
            Append(currentBuffer, startIndex, length);
        }
        #endregion

        #region Overriden System.Object Methods
        public override string ToString()
        {
            return _vArrayBuffer.ToString();
        }
        #endregion
        #endregion
    }
}
