using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Krispy.Models;

namespace Krispy.Data
{
    public class KrispyContext : DbContext
    {
        public KrispyContext (DbContextOptions<KrispyContext> options)
            : base(options)
        {
        }

        public DbSet<Krispy.Models.Sales> Sales { get; set; } = default!;
        public DbSet<Krispy.Models.Donuts> Donuts { get; set; } = default!;
    }
}
