namespace AIClassifierService.Shared.Events;

public class ParsedReceiptEvent
{
    public Guid ReceiptId { get; set; }
    public Guid UserId { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime ParsedAt { get; set; }
}
