using Microsoft.EntityFrameworkCore;
using UploadService.Domain.Entities;
using UploadService.Domain.Interfaces;
using UploadService.Infrastructure.Persistence;

namespace UploadService.Infrastructure.Repositories;

public class ReceiptRepository : IReceiptRepository
{
    private readonly UploadDbContext _context;

    public ReceiptRepository(UploadDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ReceiptFile file)
    {
        await _context.ReceiptFiles.AddAsync(file);
        await _context.SaveChangesAsync();
    }

    public async Task<ReceiptFile?> GetByIdAsync(Guid id)
    {
        return await _context.ReceiptFiles.FindAsync(id);
    }

    public async Task<IEnumerable<ReceiptFile>> GetByUserIdAsync(Guid userId)
    {
        return await _context.ReceiptFiles
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.UploadedAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(ReceiptFile file)
    {
        _context.ReceiptFiles.Update(file);
        await _context.SaveChangesAsync();
    }
}
