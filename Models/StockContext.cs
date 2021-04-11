using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckYourStocks.Models
{
    public class StockContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }

        public StockContext(DbContextOptions<StockContext> options)
            : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
