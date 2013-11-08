using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SuperMarket.SQLServer.Models;

namespace SuperMarket.SQLServer.Data
{
    public class SupermarketContext : DbContext
    {
        public SupermarketContext()
            : base("Supermarket")
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Measure> Measures { get; set; }

        public DbSet<Vendor> Vendors { get; set; }
        
        public DbSet<Sale> Sales { get; set; }
        
        public DbSet<SoldDate> SoldDates { get; set; }
        
        public DbSet<Supermarket> Supermarkets { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Measure
            //modelBuilder.Entity<Measure>().HasKey(x => x.Id);
            modelBuilder.Entity<Measure>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Measure>().Property(x => x.MeasureName).
                IsUnicode(true).HasMaxLength(50);

            // Vendor
            //modelBuilder.Entity<Vendor>().HasKey(x => x.Id);
            modelBuilder.Entity<Vendor>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Vendor>().Property(x => x.VendorName).
                IsUnicode(true).HasMaxLength(50);

            // Product
            modelBuilder.Entity<Product>().Property(x => x.ProductName).
                IsUnicode(true).HasMaxLength(80);

            // Expenses


            //// modelBuilder.Configurations.Add(new TagMappings());



            base.OnModelCreating(modelBuilder);
        }
    }
}
