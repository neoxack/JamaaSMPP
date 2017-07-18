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

namespace JamaaTech.Smpp.Net.Lib.Protocol
{
    public static class ServiceType
    {
        #region Constants
        public const string Default = ""; //Empty string
        public const string CellularMessaging = "CMT";
        public const string CellularPaging = "CPT";
        public const string VoiceMailNotification = "VMN";
        public const string VoiceMailAlerting = "VMA";
        public const string WirelessApplicationProtocol = "WAP";
        public const string UnstructuredSupplimentaryServiceData = "USSD";
        #endregion
    }
}
