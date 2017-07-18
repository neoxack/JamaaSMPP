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
using System.Collections.Generic;
using JamaaTech.Smpp.Net.Lib;
using JamaaTech.Smpp.Net.Lib.Protocol;
using JamaaTech.Smpp.Net.Lib.Util;

namespace Jamaa.Smpp.Net.Client
{
    public class TextMessage : ShortMessage
    {
        #region Variables
        private string _vText;
        private int _vMaxMessageLength;
        #endregion

        #region Constuctors
        /// <summary>
        /// Initializes a new instance of <see cref="TextMessage"/>
        /// </summary>
        public TextMessage()
        {
            _vText = "";
        }

        internal TextMessage(int segmentId, int messageCount, int sequenceNumber)
            : base(segmentId, messageCount, sequenceNumber)
        {
            _vText = "";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a <see cref="System.String"/> value representing the text content of the message
        /// </summary>
        public string Text
        {
            get { return _vText; }
            set { _vText = value; }
        }
        public int MaxMessageLength => _vMaxMessageLength;

        #endregion

        #region Methods
        protected override IEnumerable<SendSmPdu> GetPdUs(DataCoding defaultEncoding)
        {
            SubmitSm sm = new SubmitSm();
            sm.SourceAddress.Address = VSourceAddress;
            sm.DestinationAddress.Address = VDestinatinoAddress; // Urgh, typo :(
            sm.DataCoding = defaultEncoding;
            if (VRegisterDeliveryNotification)
                sm.RegisteredDelivery = RegisteredDelivery.DeliveryReceipt;

            _vMaxMessageLength = GetMaxMessageLength(defaultEncoding, false);
            byte[] bytes = SmppEncodingUtil.GetBytesFromString(_vText, defaultEncoding);

            // Unicode encoding return 2 items for 1 char 
            // We check vText Length first
            if (_vText.Length > _vMaxMessageLength && bytes.Length > _vMaxMessageLength) // Split into multiple!
            {
                var segId = new Random().Next(1000, 9999); // create random SegmentID
                _vMaxMessageLength = GetMaxMessageLength(defaultEncoding, true);
                var messages = Split(_vText, _vMaxMessageLength);
                var totalSegments = messages.Count; // get the number of (how many) parts
                var udh = new Udh(segId, totalSegments, 0); // ID, Total, part

                for (int i = 0; i < totalSegments; i++)
                {
                    udh.MessageSequence = i + 1;  // seq+1 , - parts of the message      
                    sm.SetMessageText(messages[i], defaultEncoding, udh); // send parts of the message + all other UDH settings
                    yield return sm;
                }
            }
            else
            {
                sm.SetMessageBytes(bytes);
                yield return sm;
            }
        }

        private static List<String> Split(string message, int maxPartLength)
        {
            var result = new List<String>();

            for (int i = 0; i < message.Length; i += maxPartLength)
            {
                var chunkSize = i + maxPartLength < message.Length ? maxPartLength : message.Length - i;
                var chunk = new char[chunkSize];
                message.CopyTo(i, chunk, 0, chunkSize);
                result.Add(new string(chunk));
            }

            return result;

        }

        private static int GetMaxMessageLength(DataCoding encoding, bool includeUdh)
        {
            switch (encoding)
            {
                case DataCoding.SmscDefault:
                    return includeUdh ? 153 : 160;
                case DataCoding.Latin1:
                    return includeUdh ? 134 : 140;
                case DataCoding.Ascii:
                    return includeUdh ? 153 : 160;
                case DataCoding.Ucs2:
                    return includeUdh ? 67 : 70;
                default:
                    throw new InvalidOperationException("Invalid or unsuported encoding for text message ");
            }
        }
        #endregion

        #region Overriden System.Object Members
        public override string ToString()
        {
            return _vText == null ? "" : _vText;
        }
        #endregion
    }
}
