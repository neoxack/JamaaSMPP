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

namespace JamaaTech.Smpp.Net.Lib.Protocol.Tlv
{
    public enum Tag : ushort
    {
        DestAddrSubunit = 0x0005,
        DestNetworkType = 0x0006,
        DestBearerType = 0x0007,
        DestTelematicsId = 0x0008,
        SourceAddrSubunit = 0x000D,
        SourceNetworkType = 0x000E,
        SourceBearerType = 0x000F,
        SourceTelematicsId = 0x0010,
        QosTimeToLive = 0x0017,
        PayloadType = 0x0019,
        AdditionalStatusInfoText = 0x001D,
        ReceiptedMessageId = 0x001E,
        MsMsgWaitFacilities = 0x0030,
        PrivacyIndicator = 0x0201,
        SourceSubaddress = 0x0202,
        DestSubaddress = 0x0203,
        UserMessageReference = 0x0204,
        UserResponseCode = 0x0205,
        SourcePort = 0x020A,
        DestinationPort = 0x020B,
        SarMsgRefNum = 0x020C,
        LanguageIndicator = 0x020D,
        SarTotalSegments = 0x020E,
        SarSegmentSeqnum = 0x020F,
        ScInterfaceVersion = 0x0210,
        CallbackNumPresInd = 0x0302,
        CallbackNumAtag = 0x0303,
        NumberOfMessages = 0x0304,
        CallbackNum = 0x0381,
        DpfResult = 0x0420,
        SetDpf = 0x0421,
        MsAvailabilityStatus = 0x0422,
        NetworkErrorCode = 0x0423,
        MessagePayload = 0x0424,
        DeliveryFailureReason = 0x0425,
        MoreMessagesToSend = 0x0426,
        MessageState = 0x0427,
        UssdServiceOp = 0x0501,
        DisplayTime = 0x1201,
        SmsSignal = 0x1203,
        MsValidity = 0x1204,
        AlertOnMessageDelivery = 0x130C,
        ItsReplyType = 0x1380,
        ItsSessionInfo = 0x1383
    }
}
