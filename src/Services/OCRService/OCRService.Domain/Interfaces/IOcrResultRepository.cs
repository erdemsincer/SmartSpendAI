using OCRService.Domain.Entities;

namespace OCRService.Domain.Interfaces;

public interface IOcrResultRepository
{
    Task AddAsync(OcrResult result);
    Task<OcrResult?> GetByFileIdAsync(Guid fileId);
}
