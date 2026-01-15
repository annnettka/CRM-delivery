using LogisticsCrm.Domain.Entities;

namespace LogisticsCrm.Application.Abstractions
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Client?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);

        Task AddAsync(Client client, CancellationToken cancellationToken = default);
        Task DeleteAsync(Client client, CancellationToken cancellationToken = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
