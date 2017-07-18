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

using System.Collections.Generic;
using JamaaTech.Smpp.Net.Lib;
using JamaaTech.Smpp.Net.Lib.Protocol;

namespace Jamaa.Smpp.Net.Client
{
    /// <summary>
    /// Defines a base class for diffent types of messages that can be used with <see cref="SmppClient"/>
    /// </summary>
    public abstract class ShortMessage
    {
        #region Variables
        protected string VSourceAddress;
        protected string VDestinatinoAddress;
        protected int VMessageCount;
        protected int VSegmentId;
        protected int VSequenceNumber;
        protected bool VRegisterDeliveryNotification;
        protected string VMessageId;
        #endregion

        #region Constructors

        protected ShortMessage()
        {
            VSourceAddress = "";
            VDestinatinoAddress = "";
            VSegmentId = -1;
        }

        protected ShortMessage(int segmentId, int messageCount, int sequenceNumber)
        {
            VSegmentId = segmentId;
            VMessageCount = messageCount;
            VSequenceNumber = sequenceNumber;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a <see cref="ShortMessage"/> source address
        /// </summary>
        public string SourceAddress
        {
            get { return VSourceAddress; }
            set { VSourceAddress = value; }
        }

        /// <summary>
        /// Gets or sets a <see cref="ShortMessage"/> destination address
        /// </summary>
        public string DestinationAddress
        {
            get { return VDestinatinoAddress; }
            set { VDestinatinoAddress = value; }
        }

        public string MessageId
        {
            get { return VMessageId; }
            set { VMessageId = value; }
        }

        /// <summary>
        /// Gets the index of this message segment in a group of contatenated message segements
        /// </summary>
        public int SegmentId => VSegmentId;

        /// <summary>
        /// Gets the sequence number for a group of concatenated message segments
        /// </summary>
        public int SequenceNumber => VSequenceNumber;

        /// <summary>
        /// Gets a value indicating the total number of message segments in a concatenated message
        /// </summary>
        public int MessageCount => VMessageCount;

        /// <summary>
        /// Gets or sets a <see cref="System.Boolean"/> value that indicates if a delivery notification should be sent for <see cref="ShortMessage"/>
        /// </summary>
        public bool RegisterDeliveryNotification
        {
            get { return VRegisterDeliveryNotification; }
            set { VRegisterDeliveryNotification = value; }
        }
        #endregion

        #region Methods
        internal IEnumerable<SendSmPdu> GetMessagePdUs(DataCoding defaultEncoding)
        {
            return GetPdUs(defaultEncoding);
        }

        protected abstract IEnumerable<SendSmPdu> GetPdUs(DataCoding defaultEncoding);
        #endregion
    }
}
