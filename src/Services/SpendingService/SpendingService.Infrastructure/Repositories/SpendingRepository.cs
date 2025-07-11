using Microsoft.EntityFrameworkCore;
using SpendingService.Domain.Entities;
using SpendingService.Domain.Interfaces;
using SpendingService.Infrastructure.Db;

namespace SpendingService.Infrastructure.Repositories;

public class SpendingRepository : ISpendingRepository
{
    private readonly AppDbContext _db;

    public SpendingRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(SpendingRecord record)
    {
        _db.SpendingRecords.Add(record);
        await _db.SaveChangesAsync();
    }

    public async Task<List<SpendingRecord>> GetByUserAsync(Guid userId)
    {
        return await _db.SpendingRecords
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }
}
