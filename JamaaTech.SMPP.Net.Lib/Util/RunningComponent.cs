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

namespace JamaaTech.Smpp.Net.Lib.Util
{
    public abstract class RunningComponent
    {
        #region Variables
        protected bool VRunning;
        protected object VSyncRoot;
        protected Thread VRunningThread;
        private bool _vStopOnNextCycle;
        #endregion

        #region Constructors
        public RunningComponent()
        {
            //Initit vSyncRoot
            VSyncRoot = new object();
            //vRunning = false; //false is the default boolean value anyway,  not need to set it
        }

        #endregion

        #region Properties
        public bool Running
        {
            get { lock (VSyncRoot) { return VRunning; } }
        }
        #endregion

        #region Methods
        #region Interface Methods
        public void Start()
        {
            lock (VSyncRoot) { if (VRunning) { return; } } //If this component is already running, do nothing
            //Initialize component before running owner thread
            InitializeComponent();
            RunThread();
        }

        public void Stop()
        {
            Stop(false);
        }

        public void Stop(bool allowCompleteCycle)
        {
            lock (VSyncRoot)
            {
                if (!VRunning) { return; } //If this component is stopped, do nothing
                _vStopOnNextCycle = true; //Prevent running thread from continue looping
                if (!allowCompleteCycle)
                {
                    VRunningThread.Abort(); //Abort owner thread
                    VRunningThread.Join(); //Wait until thread abort is complete
                    VRunning = false;
                    VRunningThread = null;
                }
            }
        }

        protected abstract void RunNow();

        protected virtual void ThreadCallback()
        {
            lock (VSyncRoot) { VRunning = true; }
            RunNow();
            lock (VSyncRoot)
            {
                VRunning = false;
                VRunningThread = null;
            }
        }

        protected virtual void InitializeComponent() { }

        protected virtual bool CanContinue()
        {
            lock (VSyncRoot) { return !_vStopOnNextCycle; }
        }

        protected virtual void StopOnNextCycle()
        {
            lock (VSyncRoot) { _vStopOnNextCycle = true; }
        }
        #endregion

        #region Helper Methods
        private void RunThread()
        {
            VRunningThread = new Thread(new ThreadStart(ThreadCallback));
            //Make it a background thread so that it does not keep the
            //application running after the main threads exit
            VRunningThread.IsBackground = true;
            //Start the thread
            VRunningThread.Start();
        }
        #endregion
        #endregion
    }
}
