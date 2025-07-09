using ParserService.Core.Entities;

namespace ParserService.Core.Interfaces;

public interface IParsedReceiptRepository
{
    Task AddAsync(ParsedReceipt receipt);
}
