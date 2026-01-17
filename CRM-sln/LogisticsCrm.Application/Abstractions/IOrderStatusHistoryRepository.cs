using LogisticsCrm.Domain.Entities;

namespace LogisticsCrm.Application.Abstractions
{
    public interface IOrderStatusHistoryRepository
    {
        Task AddAsync(OrderStatusHistory record, CancellationToken cancellationToken = default);
        Task<List<OrderStatusHistory>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
