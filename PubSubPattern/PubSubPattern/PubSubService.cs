using System;
using System.Linq;

namespace PubSubPattern
{
    /// <summary>
    /// A simple Pub/Sub pattern implementation.
    /// </summary>
    public sealed class PubSubService
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static PubSubService()
        {
        }

        private PubSubService()
        {
        }

        /// <summary>
        /// A singleton for service interaction.
        /// </summary>
        public static PubSubService Instance { get; } = new PubSubService();

        /// <summary>
        /// Subscribes a given IHandleMessage implementation to the channels it returns.
        /// </summary>
        /// <param name="implementation">An instance of IHandleMessage.</param>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if implementation is null</exception>
        public void Subscribe(IHandleMessage implementation)
        {
            if (implementation == null) throw new ArgumentNullException();

            var incoming = (IncomingLeadHandler)implementation;

            // if incoming is not on the subscribed list, add it to the list.
            var previouslySubscribed = SubscribedList.Subscriptions.Where(x => x.Subscriber == incoming.Subscriber).ToList().Count;

            if (previouslySubscribed == 0)
            {
                SubscribedList.Subscriptions.Add(incoming);
            }
        }

        /// <summary>
        /// Unsubscribes a given IHandleMessage implementation to the channels it returns.
        /// </summary>
        /// <param name="implementation">An instance of IHandleMessage.</param>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if implementation is null</exception>
        public void Unsubscribe(IHandleMessage implementation)
        {
            if (implementation == null) throw new ArgumentNullException();

            var tobeRemoved = (IncomingLeadHandler)implementation;

            // if tobeRemoved is already on the subscribed list, remove it from the list
            var existing = SubscribedList.Subscriptions.FirstOrDefault(x => x.Subscriber == tobeRemoved.Subscriber);

            if (existing != null)
            {
                SubscribedList.Subscriptions.Remove(existing);
            }
        }

        /// <summary>
        /// Publishes a message to a given channel containing the specified data.
        /// </summary>
        /// <param name="channel">The channel to emit a message on.</param>
        /// <param name="data">The data to emit.</param>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if channel is null.</exception>
        /// <exception cref="ArgumentNullException">Throws ArgumentNullException if data is null.</exception>
        public void Publish(string channel, object data)
        {
            if (string.IsNullOrEmpty(channel)) throw new ArgumentNullException();

            if (data == null) throw new ArgumentNullException();

            var lead = (Lead)data;

            if (string.IsNullOrEmpty(lead.FirstName) || lead.FirstName == null) throw new ArgumentException();

            if (channel == IncomingLeadHandler.INCOMING_LEAD_CHANNEL)
            {

                // if there are subscribers in the SubscribedList, process each one by doing something useful before adding lead to processed leads list. Repeat for all subscribers in the queue
                if (SubscribedList.Subscriptions.Any())
                {

                    foreach (var subscriber in SubscribedList.Subscriptions)
                    {
                        // Try doing something usefull with the message before adding lead to the processed list
                        subscriber.HandleMessage(channel, data);

                        // Add lead to the processed list
                        ProcessedLeadsStorage.ProcessedLeads.Add(lead);

                    }

                    // Since all subscribers have been taken care of, clear the list of subscribers
                    SubscribedList.Subscriptions.Clear();

                }

            }
        }
    }
}
