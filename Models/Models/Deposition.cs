using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckYourStocks.Models
{
    public class Deposition
    {
        [Key]
        public string secCode { get; set; }
        public double aweragePrice { get; set; }
        public Deposition(string secCode, double aweragePrice)
        {
            this.secCode = secCode;
            this.aweragePrice = aweragePrice;
        }
        public bool Equals(Deposition obj)
        {
            if (secCode.Equals(obj.secCode))
                return true;

            else
                return false;
        }
    }

}
