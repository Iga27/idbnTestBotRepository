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

        public override bool Equals(object obj)
        {
            FlatParameters other = (FlatParameters)obj;
            if (other == null)
            {
                return false;
            }
            return (this.Town == other.Town) && (this.Quantity == other.Quantity)
                && (this.Price==other.Price) && (this.StartYear==other.StartYear);
        }

        public override int GetHashCode()
        {
            return  Town.GetHashCode() + Quantity.GetHashCode();
        }
     

    }
}