using ParserService.Core.Entities;

namespace ParserService.Core.Interfaces;

public interface IReceiptParser
{
    ParsedReceipt Parse(Guid fileId, Guid userId, string rawText);
}
