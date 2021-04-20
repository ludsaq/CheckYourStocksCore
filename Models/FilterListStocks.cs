using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckYourStocks.Models
{
    public class FilterListStocks
    {
        public IEnumerable<Stock> Stocks { get; set; }
        public string Name { get; set; }
        public SelectList NameStocks { get; set; }
    }
}
