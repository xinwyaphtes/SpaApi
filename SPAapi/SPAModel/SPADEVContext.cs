using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SPAModel
{
    public partial class SPADEVContext : DbContext
    {
        public SPADEVContext()
        {
        }

        public SPADEVContext(DbContextOptions<SPADEVContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=SPA.DEV;uid=sa;pwd=123");
            }
        }
    }
}
