using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubPattern
{
    /// <summary>
    /// This wil contain a list of subscribed channel names
    /// </summary>
    public static class SubscribedChannelNamesStorage
    {

        static SubscribedChannelNamesStorage()
        {
            if (Names == null)
            {
                Names = new List<string>();
            }
        }

        public static List<string> Names { get; set; }
    }
}
