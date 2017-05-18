using System;

namespace Jamaa.Smpp.Net.Client
{
    public class MessageDeliveredEventArgs : EventArgs
    {

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="MessageDeliveredEventArgs"/>
        /// </summary>
        /// <param name="messageId">The messageId associated with the message event</param>
        public MessageDeliveredEventArgs(string messageId)
        {
            MessageId = messageId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the message associated with this event
        /// </summary>
        public string MessageId
        {
            get; private set;
        }

        #endregion
    }
}
