using Microsoft.EntityFrameworkCore;
using ParserService.Core.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ParserService.Infrastructure;

public class ParserDbContext : DbContext
{
    public ParserDbContext(DbContextOptions<ParserDbContext> options) : base(options) { }

    public DbSet<ParsedReceipt> ParsedReceipts => Set<ParsedReceipt>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParsedReceipt>(e =>
        {
            e.HasKey(x => x.FileId); // FileId = Unique
            e.Property(x => x.Merchant).HasMaxLength(100);
            e.Property(x => x.TotalAmount).HasPrecision(10, 2);
        });
    }
}
