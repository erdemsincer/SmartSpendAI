namespace ParserService.Core.Entities;

public class ParsedReceipt
{
    public Guid FileId { get; set; }
    public Guid UserId { get; set; }

    public string? Merchant { get; set; }
    public decimal? TotalAmount { get; set; }
    public DateTime ParsedAt { get; set; } = DateTime.UtcNow;
}
