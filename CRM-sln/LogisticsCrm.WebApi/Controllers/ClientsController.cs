using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Domain.Entities;
using LogisticsCrm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LogisticsCrm.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly LogisticsCrmDbContext _dbContext;

        public ClientRepository(LogisticsCrmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Clients
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Client?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Clients
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
        {
            await _dbContext.Clients.AddAsync(client, cancellationToken);
        }

        public Task DeleteAsync(Client client, CancellationToken cancellationToken = default)
        {
            _dbContext.Clients.Remove(client);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
