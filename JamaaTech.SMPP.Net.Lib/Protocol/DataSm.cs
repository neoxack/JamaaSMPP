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
using JamaaTech.Smpp.Net.Lib.Protocol.Tlv;

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public sealed class DataSm : SingleDestinationPdu
    {
        #region Properties
        public override SmppEntityType AllowedSource => SmppEntityType.Any;

        public override SmppSessionState AllowedSession => SmppSessionState.Transceiver;

        #endregion

        #region Constructors
        public DataSm()
            : base(new PduHeader(CommandType.DataSm)) { }

        internal DataSm(PduHeader header)
            : base(header) { }
        #endregion

        #region Methods
        public override ResponsePdu CreateDefaultResponce()
        {
            PduHeader header = new PduHeader(CommandType.DataSmResp, VHeader.SequenceNumber);
            return new DataSmResp(header);
        }

        protected override byte[] GetBodyData()
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.Append(EncodeCString(VServiceType));
            buffer.Append(VSourceAddress.GetBytes());
            buffer.Append(VDestinationAddress.GetBytes());
            buffer.Append((byte)VEsmClass);
            buffer.Append((byte)VRegisteredDelivery);
            buffer.Append((byte)VDataCoding);
            return buffer.ToBytes();
        }

        protected override void Parse(ByteBuffer buffer)
        {
            if (buffer == null) { throw new ArgumentNullException("buffer"); }
            VServiceType = DecodeCString(buffer);
            VSourceAddress = SmppAddress.Parse(buffer);
            VDestinationAddress = SmppAddress.Parse(buffer);
            VEsmClass = (EsmClass)GetByte(buffer);
            VRegisteredDelivery = (RegisteredDelivery)GetByte(buffer);
            VDataCoding = (DataCoding)GetByte(buffer);
            if (buffer.Length > 0) { VTlv = TlvCollection.Parse(buffer); }
        }

        public override byte[] GetMessageBytes()
        {
            //Check if optional parameter message_payload is present 
            Tlv.Tlv tlv = Tlv.GetTlvByTag(Tag.MessagePayload);
            if (tlv == null) { return null; }
            else { return tlv.RawValue; }
        }

        public override void SetMessageBytes(byte[] message)
        {
            if (message == null) { throw new ArgumentNullException("message"); }
            //Check if optional parameter message_payload is present 
            Tlv.Tlv tlv = Tlv.GetTlvByTag(Tag.MessagePayload);
            if (tlv == null) { throw new InvalidOperationException("Tlv parameter 'message_payload' is not present"); }
            tlv.ParseValue(message);
        }
        #endregion
    }
}
