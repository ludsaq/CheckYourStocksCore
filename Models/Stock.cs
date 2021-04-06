using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckYourStocks.Models
{
    [Keyless]
    public class Stock
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public float Cost { get; set; }
    }
}
