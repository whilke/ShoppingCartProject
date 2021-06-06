using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Service.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Item>().HasData(new Item
            {
                Id = 1,
                Name = "Google",
                Price = 1.00,
                Url = "https://www.google.com"
            });
            builder.Entity<Item>().HasData(new Item
            {
                Id = 2,
                Name = "Microsoft",
                Price = 2.00,
                Url = "https://www.microsoft.com"
            });
            builder.Entity<Item>().HasData(new Item
            {
                Id = 3,
                Name = "Yahoo",
                Price = 3.00,
                Url = "https://www.yahoo.com"
            });

            base.OnModelCreating(builder);
        }
    }
}
