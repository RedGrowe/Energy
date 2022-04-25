using KuzbassEnergo.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuzbassEnergo.Helper
{
    class Context : DbContext
    {
        public Context()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=KuzbassEnergo;Username=postgres;Password=1234");
        }

        public DbSet<Process> Processes { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<SubDivision> SubDivisions { get; set; }

    }
}
