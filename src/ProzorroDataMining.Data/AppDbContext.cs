
namespace ProzorroDataMining.Data;

using Microsoft.EntityFrameworkCore;
using ProzorroDataMining.Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tender> Tenders => Set<Tender>();
    public DbSet<TenderItem> TenderItems => Set<TenderItem>();
    public DbSet<TenderContract> TenderContracts => Set<TenderContract>();
    public DbSet<TenderAward> TenderAwards => Set<TenderAward>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}