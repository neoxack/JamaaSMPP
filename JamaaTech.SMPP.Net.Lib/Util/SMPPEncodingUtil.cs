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
    public static class SmppEncodingUtil
    {
        #region Methods
        public static byte[] GetBytesFromInt(uint value)
        {
            byte[] result = new byte[4];
            result[0] = (byte)(value >> 24);
            result[1] = (byte)(value >> 16);
            result[2] = (byte)(value >> 8);
            result[3] = (byte)(value);
            return result;
        }

        public static uint GetIntFromBytes(byte[] data)
        {
            if (data == null) { throw new ArgumentNullException("data"); }
            if (data.Length != 4) { throw new ArgumentException("Array length must be equal to four(4)", "data"); }
            uint result = 0x00000000;
            result |= data[0];
            result <<= 8;
            result |= data[1];
            result <<= 8;
            result |= data[2];
            result <<= 8;
            result |= data[3];
            return result;
        }

        public static byte[] GetBytesFromShort(ushort value)
        {
            byte[] result = new byte[2];
            result[0] = (byte)(value >> 8);
            result[1] = (byte)(value);
            return result;
        }

        public static ushort GetShortFromBytes(byte[] data)
        {
            if (data == null) { throw new ArgumentNullException("data"); }
            if (data.Length != 2) { throw new ArgumentException("Array length must be equal to two (2)", "data"); }
            ushort result = 0x0000;
            result |= data[0];
            result <<= 8;
            result |= data[1];
            return result;
        }

        public static byte[] GetBytesFromCString(string cStr)
        {
            return GetBytesFromCString(cStr, DataCoding.Ascii);
        }

        private static byte[] EncodeString(DataCoding dataCoding, string str)
        {
            byte[] bytes;
            switch (dataCoding)
            {
                case DataCoding.Ascii:
                    bytes = Encoding.ASCII.GetBytes(str);
                    break;
                case DataCoding.Latin1:
                    bytes = Latin1Encoding.GetBytes(str);
                    break;
                case DataCoding.Ucs2:
                    bytes = Encoding.BigEndianUnicode.GetBytes(str);
                    break;
                case DataCoding.SmscDefault:
                    bytes = SmscDefaultEncoding.GetBytes(str);
                    break;
                default:
                    throw new SmppException(SmppErrorCode.EsmeRunknownerr, "Unsupported encoding");
            }
            return bytes;
        }

        private static string DecodeString(byte[] data, DataCoding dataCoding)
        {
            string result;
            switch (dataCoding)
            {
                case DataCoding.Ascii:
                    result = Encoding.ASCII.GetString(data);
                    break;
                case DataCoding.Latin1:
                    result = Latin1Encoding.GetString(data);
                    break;
                case DataCoding.Ucs2:
                    result = Encoding.BigEndianUnicode.GetString(data);
                    break;
                case DataCoding.SmscDefault:
                    result = SmscDefaultEncoding.GetString(data);
                    break;              
                default:
                    throw new SmppException(SmppErrorCode.EsmeRunknownerr, "Unsupported encoding");
            }
            return result;
        }

        public static byte[] GetBytesFromCString(string cStr, DataCoding dataCoding)
        {
            if (cStr == null) { throw new ArgumentNullException("cStr"); }
            if (cStr.Length == 0) { return new byte[] { 0x00 }; }
            byte[] bytes = EncodeString(dataCoding, cStr);
            ByteBuffer buffer = new ByteBuffer(bytes, bytes.Length + 1);
            buffer.Append(new byte[] { 0x00 }); //Append a null charactor a the end
            return buffer.ToBytes();
        }

        public static string GetCStringFromBytes(byte[] data)
        {
            return GetCStringFromBytes(data, DataCoding.Ascii);
        }     

        public static string GetCStringFromBytes(byte[] data, DataCoding dataCoding)
        {
            if (data == null) { throw new ArgumentNullException("data"); }
            if (data.Length < 1) { throw new ArgumentException("Array cannot be empty","data"); }
            if (data[data.Length - 1] != 0x00) { throw new ArgumentException("CString must be terminated with a null charactor","data"); }
            if (data.Length == 1) { return ""; } //The string is empty if it contains a single null charactor
            string result = DecodeString(data, dataCoding);
            return result.Replace("\x00","");//Replace the terminating null charactor
        }

        public static byte[] GetBytesFromString(string cStr)
        {
            return GetBytesFromCString(cStr, DataCoding.Ascii);
        }

        public static byte[] GetBytesFromString(string cStr, DataCoding dataCoding)
        {
            if (cStr == null) { throw new ArgumentNullException("cStr"); }
            if (cStr.Length == 0) { return new byte[] { 0x00 }; }         
            return EncodeString(dataCoding, cStr);
        }

        public static string GetStringFromBytes(byte[] data)
        {
            return GetStringFromBytes(data, DataCoding.Ascii);
        }

        public static string GetStringFromBytes(byte[] data, DataCoding dataCoding)
        {
            if (data == null) { throw new ArgumentNullException("data"); }
            string result = DecodeString(data, dataCoding);
            //Since a CString may contain a null terminating charactor
            //Replace all occurences of null charactors
            return result.Replace("\u0000","");
        }
        
        #endregion
    }
}
