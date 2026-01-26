using LogisticsCrm.Domain.Entities;

namespace LogisticsCrm.Application.Abstractions
{
    public interface ICourierRepository
    {
        Task<List<Courier>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Courier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

       
        Task<Courier?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken = default);

        Task AddAsync(Courier courier, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
