using Microsoft.EntityFrameworkCore;
using OCRService.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OCRService.Infrastructure.Persistence;

public class OCRDbContext : DbContext
{
    public OCRDbContext(DbContextOptions<OCRDbContext> options)
        : base(options)
    {
    }

    public DbSet<OcrResult> OcrResults => Set<OcrResult>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OcrResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RawText).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
