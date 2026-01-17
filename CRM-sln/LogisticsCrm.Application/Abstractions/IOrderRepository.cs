using LogisticsCrm.Domain.Entities;

namespace LogisticsCrm.Application.Abstractions
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Order order, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<Order?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);

        Task<(List<Order> Items, int TotalCount)> SearchAsync(
        int? status,
        Guid? courierId,
        Guid? clientId,
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);


    }
}
