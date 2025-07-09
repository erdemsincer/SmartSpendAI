using ParserService.Core.Entities;
using ParserService.Core.Interfaces;

namespace ParserService.Infrastructure.Repositories;

public class ParsedReceiptRepository : IParsedReceiptRepository
{
    private readonly ParserDbContext _context;

    public ParsedReceiptRepository(ParserDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ParsedReceipt receipt)
    {
        _context.ParsedReceipts.Add(receipt);
        await _context.SaveChangesAsync();
    }
}
