namespace OCRService.Worker.Interfaces;

public interface IOcrProcessor
{
    Task<string> ProcessImageAsync(string filePath);
}
