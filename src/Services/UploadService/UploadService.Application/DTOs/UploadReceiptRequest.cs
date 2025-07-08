namespace UploadService.Application.DTOs;

public class UploadReceiptRequest
{
    public Guid UserId { get; set; }
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public int FileType { get; set; } // Enum olarak gelir (0 = Image, 1 = Pdf)
}
