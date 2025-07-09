using System.Text.RegularExpressions;
using ParserService.Core.Entities;
using ParserService.Core.Interfaces;

namespace ParserService.Core.Services;

public class SimpleReceiptParser : IReceiptParser
{
    public ParsedReceipt Parse(Guid fileId, Guid userId, string rawText)
    {
        // Basit örnek: en büyük sayı = toplam tutar
        var totalMatch = Regex.Matches(rawText, @"\d+[\.,]?\d*")
            .Select(m => decimal.TryParse(m.Value.Replace(',', '.'), out var val) ? val : 0)
            .DefaultIfEmpty()
            .Max();

        var merchant = rawText.Split('\n').FirstOrDefault()?.Trim();

        return new ParsedReceipt
        {
            FileId = fileId,
            UserId = userId,
            Merchant = merchant,
            TotalAmount = totalMatch
        };
    }
}
