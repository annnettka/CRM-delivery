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
        public async Task<(List<Order> Items, int TotalCount)> SearchAsync(
        int? status,
        Guid? courierId,
        Guid? clientId,
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100;

            var query = _dbContext.Orders.AsNoTracking().AsQueryable();

            if (status.HasValue)
                query = query.Where(o => (int)o.Status == status.Value);

            if (courierId.HasValue)
                query = query.Where(o => o.CourierId == courierId.Value);

            if (clientId.HasValue)
                query = query.Where(o => o.ClientId == clientId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(o =>
                    o.TrackingNumber.ToLower().Contains(s) ||
                    o.RecipientName.ToLower().Contains(s) ||
                    (o.RecipientPhone != null && o.RecipientPhone.ToLower().Contains(s)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(o => o.CreatedAtUtc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

    }
}
