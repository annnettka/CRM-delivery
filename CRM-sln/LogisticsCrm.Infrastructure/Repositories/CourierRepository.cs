using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Domain.Entities;
using LogisticsCrm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsCrm.Infrastructure.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly LogisticsCrmDbContext _dbContext;

        public CourierRepository(LogisticsCrmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Courier>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Couriers
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Courier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Couriers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Courier?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Couriers
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken); 
        }

        public async Task AddAsync(Courier courier, CancellationToken cancellationToken = default)
        {
            await _dbContext.Couriers.AddAsync(courier, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
