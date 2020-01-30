using System;
using System.Collections.Generic;
using System.Text;
using ComicBookInventory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComicBookInventory.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<ApplicationUser> Users { get; set; }
            public DbSet<Comic> Comics { get; set; }

        public DbSet<ComicBookInventory.Models.Wishlist> Wishlist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                //Create User
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    NormalizedUserName = "ADMIN@ADMIN.COM",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794577",
                    Id = "00000000-ffff-ffff-ffff-ffffffffffff",
                    //FirstName = "Justin",
                    //LastName = "Warden",
                };

                var passwordHash = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = passwordHash.HashPassword(user, "Admin123!");
                modelBuilder.Entity<ApplicationUser>().HasData(user);

            // Create four Comics
            modelBuilder.Entity<Comic>().HasData(
                new Comic()
                {
                    Id = 1,
                    UserId = "00000000-ffff-ffff-ffff-ffffffffffff",
                    Title = "X-Men",
                    Publisher = "Marvel",
                    Year = 1965,
                    VolumeNumber = 1,
                    Price = 0.65,
                    Notes = "Wow what a fun book!!!",
                    ComicImage = null,
                },

                    new Comic()
                    {
                        Id = 2,
                        UserId = "00000000-ffff-ffff-ffff-ffffffffffff",
                        Title = "Swamp Thing",
                        Publisher = "DC",
                        Year = 1980,
                        VolumeNumber = 2,
                        Price = 1.65,
                        Notes = "He sure is slimy",
                        ComicImage = null,
                    },

                             new Comic()
                             {
                                 Id = 3,
                                 UserId = "00000000-ffff-ffff-ffff-ffffffffffff",
                                 Title = "Spawn",
                                 Publisher = "Image",
                                 Year = 1991,
                                 VolumeNumber = 1,
                                 Price = 2.00,
                                 Notes = "",
                                 ComicImage = null,
                             }
                             );

                                  

                //modelBuilder.Entity<Order>().HasMany(Order => Order.OrderProducts)
                //.WithOne(OrderProducts => OrderProducts.Order)
                //.OnDelete(DeleteBehavior.Restrict);

            //// Restrict deletion of related product when OrderProducts entry is removed
            //modelBuilder.Entity<Product>()
            //    .HasMany(Order => Order.OrderProducts)
            //    .WithOne(OrderProducts => OrderProducts.Product)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Product>()
            //    .Property(D => D.DateCreated)
            //    .HasDefaultValueSql("GETDATE()");
        }
         
        }
    }