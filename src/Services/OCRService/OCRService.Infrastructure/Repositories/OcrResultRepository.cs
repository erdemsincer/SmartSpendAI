using Microsoft.EntityFrameworkCore;
using OCRService.Domain.Entities;
using OCRService.Domain.Interfaces;
using OCRService.Infrastructure.Persistence;

namespace OCRService.Infrastructure.Repositories;

public class OcrResultRepository : IOcrResultRepository
{
    private readonly OCRDbContext _context;

    public OcrResultRepository(OCRDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OcrResult result)
    {
        _context.OcrResults.Add(result);
        await _context.SaveChangesAsync();
    }

    public async Task<OcrResult?> GetByFileIdAsync(Guid fileId)
    {
        return await _context.OcrResults.FirstOrDefaultAsync(x => x.FileId == fileId);
    }
}
