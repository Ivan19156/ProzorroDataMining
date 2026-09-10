// Data/Configurations/TenderContractConfiguration.cs
namespace ProzorroDataMining.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProzorroDataMining.Domain.Entities;

public sealed class TenderContractConfiguration : IEntityTypeConfiguration<TenderContract>
{
    public void Configure(EntityTypeBuilder<TenderContract> builder)
    {
        builder.ToTable("tender_contracts");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnType("varchar(32)")
            .IsRequired();

        builder.Property(c => c.TenderId)
            .HasColumnType("varchar(32)")
            .IsRequired();

        builder.Property(c => c.ContractValue)
            .HasColumnType("numeric(18,2)");

        builder.Property(c => c.Currency)
            .HasColumnType("varchar(3)")
            .HasDefaultValue("UAH");

        builder.Property(c => c.Status)
            .HasColumnType("varchar(20)");

        builder.Property(c => c.DateSigned)
            .HasColumnType("timestamptz");

        builder.HasIndex(c => c.TenderId)
            .HasDatabaseName("idx_tender_contracts_tender_id");
    }
}