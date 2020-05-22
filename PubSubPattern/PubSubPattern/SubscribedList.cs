using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubPattern
{
    /// <summary>
    /// This is where all the valid subscribers are queued for processing
    /// </summary>
    public static class SubscribedList
    {
        static SubscribedList()
        {
            if (Subscriptions == null)
            {
                Subscriptions = new List<IncomingLeadHandler>();
            }
        }

        public static List<IncomingLeadHandler> Subscriptions { get; set; }
    }
}
