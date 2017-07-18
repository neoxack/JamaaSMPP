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

using System.Threading;

namespace JamaaTech.Smpp.Net.Lib
{
    internal class PduWaitContext
    {
        #region Variables

        private readonly AutoResetEvent _vNotifyEvent;
        private readonly int _vTimeOut;
        private bool _vTimedOut;
        #endregion

        #region Constructors
        public PduWaitContext(uint sequenceNumber,int timeOut)
        {
            SequenceNumber = sequenceNumber;
            _vNotifyEvent = new AutoResetEvent(false);
            _vTimeOut = timeOut;
        }
        #endregion

        #region Properties
        public uint SequenceNumber { get; }

        public bool TimedOut => _vTimedOut;

        #endregion

        #region Methods
        public bool WaitForAlert()
        {
            _vTimedOut = !_vNotifyEvent.WaitOne(_vTimeOut,false);
            return !_vTimedOut;
        }

        public void AlertResponseReceived()
        {
            _vNotifyEvent.Set();
        }
        #endregion
    }
}
