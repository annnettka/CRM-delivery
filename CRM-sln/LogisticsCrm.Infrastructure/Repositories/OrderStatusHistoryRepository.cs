using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Domain.Entities;
using LogisticsCrm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsCrm.Infrastructure.Repositories
{
    public class OrderStatusHistoryRepository : IOrderStatusHistoryRepository
    {
        private readonly LogisticsCrmDbContext _dbContext;

        public OrderStatusHistoryRepository(LogisticsCrmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(OrderStatusHistory record, CancellationToken cancellationToken = default)
        {
            await _dbContext.OrderStatusHistory.AddAsync(record, cancellationToken);
        }

        public async Task<List<OrderStatusHistory>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.OrderStatusHistory
                .AsNoTracking()
                .Where(x => x.OrderId == orderId)
                .OrderBy(x => x.ChangedAtUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
