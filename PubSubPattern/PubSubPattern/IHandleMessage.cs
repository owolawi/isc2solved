using System;
using System.Collections.Generic;

namespace PubSubPattern
{
    public interface IHandleMessage 
    {
        /// <summary>
        /// Name of the Subscriber.
        /// </summary>
        string Subscriber { get; set; }

        /// <summary>
        /// Handles a message on a subscribed channel.
        /// </summary>
        /// <param name="channel">The channel emitting the message.</param>
        /// <param name="data">The accompanying data for the message.</param>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if channel is null.</exception>
        void HandleMessage(string channel, object data);

        /// <summary>
        /// Gets a list of channels an implementation subscribes to. 
        /// </summary>
        /// <returns>Returns a list of string channel names this implementation is subscribed to.</returns>
        List<string> GetSubscribedChannels();
    }
}
