using SpendingService.Domain.Entities;

namespace SpendingService.Domain.Interfaces;

public interface ISpendingRepository
{
    Task AddAsync(SpendingRecord record);
    Task<List<SpendingRecord>> GetByUserAsync(Guid userId);
}
