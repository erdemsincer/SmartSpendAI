using UploadService.Domain.Entities;

namespace UploadService.Domain.Interfaces;

public interface IReceiptRepository
{
    Task AddAsync(ReceiptFile file);
    Task<ReceiptFile?> GetByIdAsync(Guid id);
    Task<IEnumerable<ReceiptFile>> GetByUserIdAsync(Guid userId);
    Task UpdateAsync(ReceiptFile file);  // isProcessed flag'i için
}
