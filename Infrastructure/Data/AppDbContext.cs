using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(eb =>
            {
                eb.HasKey(e => e.Id);
                eb.Property(e => e.Name).IsRequired();
                eb.Property(e => e.Category).IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (Category)Enum.Parse(typeof(Category), v));
                eb.Property(e => e.Price).IsRequired();
                eb.Property(e => e.StockQuantity).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
