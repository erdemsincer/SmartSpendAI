using MediatR;

namespace SpendingService.Application.Commands;

public class AddSpendingCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid ReceiptId { get; set; }
    public string Category { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
}
