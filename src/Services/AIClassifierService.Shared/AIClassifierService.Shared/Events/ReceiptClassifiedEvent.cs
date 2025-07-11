namespace SpendingService.Shared.Events;

public class ReceiptClassifiedEvent
{
    public Guid UserId { get; set; }
    public Guid ReceiptId { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ClassifiedAt { get; set; }
}
