using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Core.Entities;

namespace test.Core
{
    public class DellinDictionaryDbContext : DbContext
    {
        public virtual DbSet<Office> Offices { get; set; }

        public virtual DbSet<Phone> Phones { get; set; }

        public virtual DbSet<Coordinate> Coordinates { get; set; }

        public DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Office>()
                .Property(u => u.Code)
                .HasMaxLength(25);

            modelBuilder.Entity<Office>()
                .Property(u => u.Uuid)
                .HasMaxLength(255);

            modelBuilder.Entity<Office>()
                .Property(u => u.CountryCode)
                .HasMaxLength(3);

            modelBuilder.Entity<Office>()
                .Property(u => u.WorkTime)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Office>()
                .OwnsOne(p => p.Address);

            modelBuilder.Entity<Office>()
                .OwnsOne(p => p.Coordinate);

            modelBuilder.Entity<Office>()
                .HasOne(u => u.Phones)
                .WithOne(p => p.Office)
                .HasForeignKey<Phone>(p => p.OfficeId);

            modelBuilder.Entity<Phone>()
                .Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Phone>()
                .Property(u => u.Additional)
                .HasMaxLength(50);
/*
            modelBuilder.Entity<Coordinate>()
                .HasNoKey();
            /*
            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .HasMaxLength(255);

            modelBuilder.Entity<Address>()
                .Property(a => a.HouseNumber)
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(a => a.Region)
                .HasMaxLength(255);

            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .HasMaxLength(255);*/
        }


    }
}
