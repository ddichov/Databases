using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarket.SQLite.Models;

namespace SuperMarket.SQLite.Data
{
    public class SupermarketSQLiteContext : DbContext
    {
        public SupermarketSQLiteContext()
            : base("SupermarketSQLite")
        {
        }

        public DbSet<TaxTable> Taxes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxTable>().Property(x => x.Id);
            modelBuilder.Entity<TaxTable>().Property(x => x.ProductName).
                IsUnicode(true).HasMaxLength(50);

            //// modelBuilder.Configurations.Add(new TagMappings());

            base.OnModelCreating(modelBuilder);
        }
    }
}
