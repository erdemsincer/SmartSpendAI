namespace OCRService.Domain.Entities;

public class OcrResult
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid FileId { get; set; }

    public Guid UserId { get; set; }

    public string RawText { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
