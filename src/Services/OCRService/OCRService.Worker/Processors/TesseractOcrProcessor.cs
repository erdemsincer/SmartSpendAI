using OCRService.Worker.Interfaces;
using Tesseract;

namespace OCRService.Worker.Processors;

public class TesseractOcrProcessor : IOcrProcessor
{
    public async Task<string> ProcessImageAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine($"[⚠️] Geçersiz veya bulunamayan dosya yolu: {filePath}");
            return string.Empty;
        }

        return await Task.Run(() =>
        {
            try
            {
                using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
                using var img = Pix.LoadFromFile(filePath);
                using var page = engine.Process(img);
                return page.GetText();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[❌] OCR işleminde hata oluştu: {ex.Message}");
                return string.Empty;
            }
        });
    }
}
