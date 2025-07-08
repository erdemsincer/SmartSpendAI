using Microsoft.EntityFrameworkCore;
using UploadService.Domain.Entities;

namespace UploadService.Infrastructure.Persistence;

public class UploadDbContext : DbContext
{
    public UploadDbContext(DbContextOptions<UploadDbContext> options) : base(options) { }

    public DbSet<ReceiptFile> ReceiptFiles => Set<ReceiptFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReceiptFile>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FileName).IsRequired().HasMaxLength(255);
            entity.Property(x => x.FilePath).IsRequired();
            entity.Property(x => x.UserId).IsRequired();
            entity.Property(x => x.FileType).IsRequired();
            entity.Property(x => x.IsProcessed).IsRequired();
        });
    }
}
