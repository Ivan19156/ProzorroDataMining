namespace ProzorroDataMining.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProzorroDataMining.Domain.Entities;

public sealed class TenderConfiguration : IEntityTypeConfiguration<Tender>
{
    public void Configure(EntityTypeBuilder<Tender> builder)
    {
        builder.ToTable("tenders");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasColumnType("varchar(32)")
            .IsRequired();

        builder.Property(t => t.Status)
            .HasColumnType("varchar(20)")
            .IsRequired();

        builder.Property(t => t.BudgetAmount)
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(t => t.BudgetCurrency)
            .HasColumnType("varchar(3)")
            .HasDefaultValue("UAH");

        builder.Property(t => t.ProcuringEntityId)
            .HasColumnType("varchar(50)");

        builder.Property(t => t.ProcuringEntityName)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(t => t.DateCreated)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(t => t.DateModified)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(t => t.FetchedAt)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.HasIndex(t => t.Status)
            .HasDatabaseName("idx_tenders_status");

        builder.HasIndex(t => t.DateCreated)
            .HasDatabaseName("idx_tenders_date_created");

        builder.HasIndex(t => new { t.Status, t.DateCreated })
            .HasDatabaseName("idx_tenders_status_date");

        builder.HasIndex(t => t.ProcuringEntityName)
            .HasDatabaseName("idx_tenders_entity_name");

        builder.HasMany(t => t.Items)
            .WithOne(i => i.Tender)
            .HasForeignKey(i => i.TenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Contracts)
            .WithOne(c => c.Tender)
            .HasForeignKey(c => c.TenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Awards)
            .WithOne(a => a.Tender)
            .HasForeignKey(a => a.TenderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}