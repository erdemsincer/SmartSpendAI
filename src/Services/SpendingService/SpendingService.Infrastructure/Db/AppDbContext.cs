using Microsoft.EntityFrameworkCore;
using SpendingService.Domain.Entities;
using System.Collections.Generic;

namespace SpendingService.Infrastructure.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SpendingRecord> SpendingRecords => Set<SpendingRecord>();
}
