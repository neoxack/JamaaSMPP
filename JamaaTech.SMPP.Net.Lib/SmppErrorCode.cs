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

namespace JamaaTech.Smpp.Net.Lib
{
    public enum SmppErrorCode : uint
    {
        /// <summary>
        /// No error
        /// </summary>
        EsmeRok = 0x00000000,
        /// <summary>
        /// Message length is invalid
        /// </summary>
        EsmeRinvmsglen = 0x00000001,
        /// <summary>
        /// Command length is invalid
        /// </summary>
        EsmeRinvcmdlen = 0x00000002,
        /// <summary>
        /// Invalid command ID
        /// </summary>
        EsmeRinvcmdid = 0x00000003,
        /// <summary>
        /// Incorrect bind status for given command
        /// </summary>
        EsmeRinvbndsts = 0x00000004,
        /// <summary>
        /// ESME already in bound state
        /// </summary>
        EsmeRalybnd = 0x00000005,
        /// <summary>
        /// Invalid priority flag
        /// </summary>
        EsmeRinvprtflg = 0x00000006,
        /// <summary>
        /// Invalid registred delivery flag
        /// </summary>
        EsmeRinvregdlvflg = 0x00000007,
        /// <summary>
        /// System error
        /// </summary>
        EsmeRsyserr = 0x00000008,
        /// <summary>
        /// Invalid source address
        /// </summary>
        EsmeRinvsrcadr = 0x0000000A,
        /// <summary>
        /// Invalid destination address
        /// </summary>
        EsmeRinvdstadr = 0x0000000B,
        /// <summary>
        /// Message ID is invalid
        /// </summary>
        EsmeRinvmsgid = 0x0000000C,
        /// <summary>
        /// Bind failed
        /// </summary>
        EsmeRbindfail = 0x0000000D,
        /// <summary>
        /// Invlaid password
        /// </summary>
        EsmeRinvpaswd = 0x0000000E,
        /// <summary>
        /// Invalid system ID
        /// </summary>
        EsmeRinvsysid = 0x0000000F,
        /// <summary>
        /// Cancel SM failed
        /// </summary>
        EsmeRcancelfail = 0x00000011,
        /// <summary>
        /// Replace SM failed
        /// </summary>
        EsmeRreplacefail = 0x00000013,
        /// <summary>
        /// Message queue full
        /// </summary>
        EsmeRmsgqful = 0x00000014,
        /// <summary>
        /// Invalid service type
        /// </summary>
        EsmeRinvsertyp = 0x00000015,
        /// <summary>
        /// Invalid number of destinations
        /// </summary>
        EsmeRinvnumdests = 0x00000033,
        /// <summary>
        /// Invalid distribution list name
        /// </summary>
        EsmeRinvdlname = 0x00000034,
        /// <summary>
        /// Destination flag is invalid
        /// </summary>
        EsmeRinvdestflag = 0x00000040,
        /// <summary>
        /// Invalid submit with replace request
        /// </summary>
        EsmeRinvsubrep = 0x00000042,
        /// <summary>
        /// Invalid esm_class field data
        /// </summary>
        EsmeRinvesmclass = 0x00000043,
        /// <summary>
        /// Cannot submit to distribution list
        /// </summary>
        EsmeRcntsubdl = 0x00000044,
        /// <summary>
        /// submit_sm or submit_multi failed
        /// </summary>
        EsmeRsubmitfail = 0x00000045,
        /// <summary>
        /// Invalid source address TON
        /// </summary>
        EsmeRinvsrcton = 0x00000048,
        /// <summary>
        /// Invalid source address NPI
        /// </summary>
        EsmeRinvsrcnpi = 0x00000049,
        /// <summary>
        /// Invalid destionation address TON
        /// </summary>
        EsmeRinvdstton = 0x00000050,
        /// <summary>
        /// Invalid destination address NPI
        /// </summary>
        EsmeRinvdstnpi = 0x00000051,
        /// <summary>
        /// Invalid system type field
        /// </summary>
        EsmeRinvsystyp = 0x00000053,
        /// <summary>
        /// Invalid replace if present flag
        /// </summary>
        EsmeRinvrepflag = 0x00000054,
        /// <summary>
        /// Invlalid number of messages
        /// </summary>
        EsmeRinvnummsgs = 0x00000055,
        /// <summary>
        /// Throttling error. ESME has exceeded allowed message limits
        /// </summary>
        EsmeRthrottled = 0x00000058,
        /// <summary>
        /// Invalid schedule delivery time
        /// </summary>
        EsmeRinvsched = 0x00000061,
        /// <summary>
        /// Invalid message validity period
        /// </summary>
        EsmeRinvexpiry = 0x00000062,
        /// <summary>
        /// Predefined message invalid or not found
        /// </summary>
        EsmeRinvdftmsgid = 0x00000063,
        /// <summary>
        /// ESME receiver temporary application error
        /// </summary>
        EsmeRxTAppn = 0x00000064,
        /// <summary>
        /// ESME receiver permanent application error
        /// </summary>
        EsmeRxPAppn = 0x00000065,
        /// <summary>
        /// ESME receiver reject message error
        /// </summary>
        EsmeRxRAppn = 0x00000066,
        /// <summary>
        /// query_sm failed
        /// </summary>
        EsmeRqueryfail = 0x00000067,
        /// <summary>
        /// Error in the optional part of the pdu body
        /// </summary>
        EsmeRinvoptparstream = 0x000000C0,
        /// <summary>
        /// Optional parameter not allowed
        /// </summary>
        EsmeRoptparnotallwd = 0x000000C1,
        /// <summary>
        /// Invalid parameter length
        /// </summary>
        EsmeRinvparlen = 0x000000C2,
        /// <summary>
        /// Expected optinal parameter missing
        /// </summary>
        EsmeRmissingoptparam = 0x000000C3,
        /// <summary>
        /// Invalid optional parameter value
        /// </summary>
        EsmeRinvoptparamval = 0x000000C4,
        /// <summary>
        /// Delivery failure
        /// </summary>
        EsmeRdeliveryfailure = 0x000000FE,
        /// <summary>
        /// Unknown error
        /// </summary>
        EsmeRunknownerr = 0x000000FF
    }
}
