namespace ProzorroDataMining.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProzorroDataMining.Domain.Entities;

public sealed class TenderAwardConfiguration : IEntityTypeConfiguration<TenderAward>
{
    public void Configure(EntityTypeBuilder<TenderAward> builder)
    {
        builder.ToTable("tender_awards");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnType("varchar(32)")
            .IsRequired();

        builder.Property(a => a.TenderId)
            .HasColumnType("varchar(32)")
            .IsRequired();

        builder.Property(a => a.AwardValue)
            .HasColumnType("numeric(18,2)");

        builder.Property(a => a.Status)
            .HasColumnType("varchar(20)");

        builder.Property(a => a.SupplierName)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(a => a.SupplierId)
            .HasColumnType("varchar(50)");

        builder.HasIndex(a => a.TenderId)
            .HasDatabaseName("idx_tender_awards_tender_id");

        builder.HasIndex(a => a.SupplierName)
            .HasDatabaseName("idx_tender_awards_supplier_name");
    }
}