using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ranna.Task2.DataAccess.Seeds;
using Ranna.Task2.Entities.Models;

namespace Ranna.Task2.DataAccess.Configurations
{
	internal class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.ToTable("Urunler")
				.HasKey(x => x.Id);
			builder.Property(m=>m.Ad)
				.IsRequired()
				.HasMaxLength(100);
			builder.Property(m => m.Kod)
				.IsRequired()
				.HasMaxLength(50);
			builder.Property(m => m.Fiyat)
				.IsRequired()
				.HasColumnType("decimal(18,2)");
			builder.HasIndex(m => m.Kod)
				.IsUnique();
			builder.HasData(UrunSeed.Urunler);
		}
	}
}
