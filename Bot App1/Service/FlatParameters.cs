using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_App1.Service
{
    [Serializable]
    public class FlatParameters
    {
        public string Town { get; set; }

        public string Quantity { get; set; }

        public string StartYear { get; set; }

        public string Price { get; set; }
    }
}