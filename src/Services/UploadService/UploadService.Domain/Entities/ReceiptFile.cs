using UploadService.Domain.Enums;

namespace UploadService.Domain.Entities;

public class ReceiptFile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }  // Dosya kime ait

    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public FileType FileType { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public bool IsProcessed { get; set; } = false;  // OCR yapıldı mı?
}
