using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckYourStocks.Models
{
    public class listMoney
    {
        public static List<Deposition> listAweragePrice { get; set; }
        public static Dictionary<string, decimal> listHistory { get; set; }
        static listMoney() 
        {
            listAweragePrice = new List<Deposition>();
            listHistory = new Dictionary<string, decimal>();
        }
    }
}
