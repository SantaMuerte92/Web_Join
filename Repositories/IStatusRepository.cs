using Web_Join.Models;

namespace Web_Join.Repositories;

public interface IStatusRepository
{
    Task<List<Status>> GetAllAsync(CancellationToken ct = default);
    Task<Status?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Status> AddAsync(Status entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(Status entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}