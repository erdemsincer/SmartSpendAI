using OCRService.Worker.Interfaces;
using Tesseract;

namespace OCRService.Worker.Processors;

public class TesseractOcrProcessor : IOcrProcessor
{
    public async Task<string> ProcessImageAsync(string filePath)
    {
        return await Task.Run(() =>
        {
            using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
            using var img = Pix.LoadFromFile(filePath);
            using var page = engine.Process(img);
            return page.GetText();
        });
    }
}
