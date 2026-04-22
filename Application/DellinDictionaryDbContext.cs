using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Application
{
    public class DellinDictionaryDbContext : DbContext
    {
        public DbSet<Office> Offices { get; set; }
        public DbSet<Phone> Phones { get; set; }

        public DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Office>(e =>
            {
                e.ToTable("Offices");
                e.HasKey(x => x.Id);;

                e.HasIndex(x => x.AddressCity);
                e.HasIndex(x => x.AddressRegion);
                e.HasIndex(x => new { x.AddressRegion, x.AddressCity });
            });

            builder.Entity<Phone>(e =>
            {
                e.ToTable("Phones");
                e.HasKey(x => x.Id);

                e.HasOne(x => x.Office)
                    .WithMany(x => x.Phones)
                    .HasForeignKey(x => x.OfficeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
