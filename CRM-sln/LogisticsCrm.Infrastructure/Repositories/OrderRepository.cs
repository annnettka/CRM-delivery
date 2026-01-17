using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Domain.Entities;
using LogisticsCrm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsCrm.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly LogisticsCrmDbContext _dbContext;

        public OrderRepository(LogisticsCrmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _dbContext.Orders.AddAsync(order, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Order?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

    }
}
