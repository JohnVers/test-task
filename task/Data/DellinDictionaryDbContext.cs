using Microsoft.EntityFrameworkCore;
using task.Models;

namespace task.Data;

public class DellinDictionaryDbContext : DbContext
{
	public DellinDictionaryDbContext(DbContextOptions<DellinDictionaryDbContext> options)
		: base(options) { }

	public DbSet<Office> Offices => Set<Office>();
	public DbSet<Phone> Phones => Set<Phone>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Office>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.Property(e => e.Id).UseIdentityColumn();

			entity.OwnsOne(e => e.Coordinates, coords =>
			{
				coords.Property(c => c.Latitude).HasColumnName("Latitude");
				coords.Property(c => c.Longitude).HasColumnName("Longitude");
			});

			// Индекс для поиска по городу (покрывающий CityCode)
			entity.HasIndex(e => e.AddressCity)
				  .IncludeProperties(e => e.CityCode)
				  .HasDatabaseName("IX_Offices_AddressCity_CityCode");

			// Дополнительный индекс для поиска по CityCode
			entity.HasIndex(e => e.CityCode)
				  .HasDatabaseName("IX_Offices_CityCode");
		});
	}
}