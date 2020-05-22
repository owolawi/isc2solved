using System;
using System.Collections.Generic;

namespace PubSubPattern
{
    /// <summary>
    /// An implementation of IHandleMessage which validates the passed in Lead and inserts it.
    /// </summary>
    public class IncomingLeadHandler : IHandleMessage
    {
        public static readonly string INCOMING_LEAD_SUBSCRIBER = "IncomingLeadsSubscriber";

        public static readonly string INCOMING_LEAD_CHANNEL = "IncomingLeads";

        /// <summary>
        /// Name of the Subscriber.
        /// </summary>
        public string Subscriber { get; set; }

        /// <summary>
        /// Constructs an instance of IncomingLeadHandler.
        /// </summary>
        public IncomingLeadHandler(string subscriber = null)
        {
            Subscriber = subscriber == null ? INCOMING_LEAD_SUBSCRIBER : subscriber;
        }

        /// <summary>
        /// Overrides the default comparison logic to properly define the equality for IHandleMessage type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Returns true if the current object is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            IHandleMessage handle = obj as IHandleMessage;
            return handle != null && handle.Subscriber == Subscriber;
        }

        /// <summary>
        /// Overrides the default implementation to generate a hash based on the values within a class.
        /// </summary>
        /// <returns>returns a 32-bit signed integer hash code</returns>
        public override int GetHashCode()
        {
            return Subscriber.GetHashCode();
        }

        /// <summary>
        /// Handles a message on a subscribed channel.
        /// </summary>
        /// <param name="channel">The channel emitting the message.</param>
        /// <param name="data">The accompanying data for the message.</param>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if channel is null.</exception>
        /// <exception cref="ArgumentException">Throws ArgumentException if the lead is missing a FirstName or if it is empty.</exception>
        public void HandleMessage(string channel, object data)
        {

            if (string.IsNullOrEmpty(channel) || data == null) throw new ArgumentNullException();

            var lead = (Lead)data;

            if(string.IsNullOrEmpty(lead.FirstName)) throw new ArgumentNullException();

            var subscribedChannels = GetSubscribedChannels();

            // If the channel is not on the list of names add it to the list - alternatively, one could just add to the list without checking if it already exists on the list, depending on requirements.

            if (!subscribedChannels.Contains(channel)) SubscribedChannelNamesStorage.Names.Add(channel);

            // Do other useful stuff with the channel and lead data

        }

        /// <summary>
        /// Gets a list of channels an implementation subscribes to.
        /// </summary>
        /// <returns>Returns a list of string channel names this implementation is subscribed to.</returns>
        public List<string> GetSubscribedChannels()
        {
            return SubscribedChannelNamesStorage.Names;
            
        }
    }
}
