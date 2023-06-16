using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    public class CoreDBContext : IdentityDbContext<ApplicationUser>
    {//pass DBContextOptions u base class konstruktor!!!
        public CoreDBContext(DbContextOptions<CoreDBContext> options) : base(options)
        {

        }

        //definisanje entitija sa kojima se CoreDBContext da komunicira!

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<OpeningHours> OpeningHourss { get; set; }
        public DbSet<Shipping> Shippings { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Order>().HasKey(or => new { or.OrderId, or.OrderGuid });
            //modelBuilder.Entity<Category>().HasData(
            //    new Category
            //    {
            //        CategoryId = 1,
            //        CategoryName = "Laptops"



            //    }

            //    );

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {

                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<OrderItem>().HasOne(x => x.Product).WithMany(x => x.OrderItems).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany(x => x.Products).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>().HasOne(x => x.Store).WithMany(x => x.Products).OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Category>().HasOne(x => x.Store).WithMany(x => x.Categories).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<OpeningHours>().HasOne(x => x.Store).WithMany(x => x.OpeningHourss).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Shipping>().HasOne(x => x.Store).WithMany(x => x.Shippings).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Order>().HasOne(x => x.Store).WithMany(x => x.Orders).OnDelete(DeleteBehavior.SetNull);

        }
    }
}
