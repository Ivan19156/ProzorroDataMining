
namespace ProzorroDataMining.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProzorroDataMining.Domain.Entities;

public sealed class TenderItemConfiguration : IEntityTypeConfiguration<TenderItem>
{
    public void Configure(EntityTypeBuilder<TenderItem> builder)
    {
        builder.ToTable("tender_items");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasColumnType("varchar(32)")
            .IsRequired();

        builder.Property(i => i.TenderId)
            .HasColumnType("varchar(32)")
            .IsRequired();

        builder.Property(i => i.CpvCode)
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder.Property(i => i.CpvDescription)
            .HasColumnType("text");

        builder.Property(i => i.Description)
            .HasColumnType("text");

        builder.Property(i => i.Quantity)
            .HasColumnType("numeric(10,2)");

        builder.Property(i => i.UnitName)
            .HasColumnType("varchar(100)");

        builder.Property(i => i.UnitCode)
            .HasColumnType("varchar(20)");

        builder.HasIndex(i => i.TenderId)
            .HasDatabaseName("idx_tender_items_tender_id");

        builder.HasIndex(i => i.CpvCode)
            .HasDatabaseName("idx_tender_items_cpv_code");
    }
}