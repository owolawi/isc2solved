using System.Collections.Generic;

namespace PubSubPattern
{
    public static class ProcessedLeadsStorage
    {
        static ProcessedLeadsStorage()
        {
            if (ProcessedLeads == null)
            {
                ProcessedLeads = new List<Lead>();
            }  
        }

        public static List<Lead> ProcessedLeads { get; set; }
    }
}
