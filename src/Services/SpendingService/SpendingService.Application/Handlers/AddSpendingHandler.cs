using MediatR;
using SpendingService.Application.Commands;
using SpendingService.Domain.Entities;
using SpendingService.Domain.Interfaces;

namespace SpendingService.Application.Handlers;

public class AddSpendingHandler : IRequestHandler<AddSpendingCommand, Unit>

{
    private readonly ISpendingRepository _repository;

    public AddSpendingHandler(ISpendingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(AddSpendingCommand request, CancellationToken cancellationToken)
    {
        var spending = new SpendingRecord
        {
            UserId = request.UserId,
            ReceiptId = request.ReceiptId,
            Category = request.Category,
            Amount = request.Amount,
            Timestamp = request.Timestamp
        };

        await _repository.AddAsync(spending);
        return Unit.Value;
    }
}
