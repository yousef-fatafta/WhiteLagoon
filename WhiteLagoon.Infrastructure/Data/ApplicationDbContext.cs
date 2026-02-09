using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Description = "This is the description of the Royal Villa",
                    Price = 200.0,
                    Sqft = 550,
                    Occupancy = 4,
                    ImageUrl = "https://placehold.co/600x400",
                },
                new Villa
                {
                    Id = 2,
                    Name = "Premium Pool Villa",
                    Description = "This is the description of the Premium Pool Villa",
                    Price = 300.0,
                    Sqft = 550,
                    Occupancy = 4,
                    ImageUrl = "https://placehold.co/600x401",
                },
                new Villa
                {
                    Id = 3,
                    Name = "Luxury Pool Villa",
                    Description = "This is the description of the Luxury Pool Villa",
                    Price = 400.0,
                    Sqft = 750,
                    Occupancy = 4,
                    ImageUrl = "https://placehold.co/600x402",
                }
            );
        }
    }
}
