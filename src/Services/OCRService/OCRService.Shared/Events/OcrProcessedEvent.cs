namespace OCRService.Shared.Events;

public class OcrProcessedEvent
{
    public Guid FileId { get; set; }
    public Guid UserId { get; set; }
    public string RawText { get; set; } = null!;
}
