namespace OCRService.Shared.Events;

public class FileUploadedEvent
{
    public Guid FileId { get; set; }
    public Guid UserId { get; set; }
    public string FilePath { get; set; } = null!;
}
    