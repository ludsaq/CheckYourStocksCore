using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckYourStocks.Models
{
    public class DepositionContext : DbContext
    {
        public DbSet<Deposition> Depositions { get; set; }

        public DepositionContext(DbContextOptions<DepositionContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
