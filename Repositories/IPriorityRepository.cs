using Web_Join.Models;

namespace Web_Join.Repositories;

public interface IPriorityRepository
{
    Task<List<Priority>> GetAllAsync(CancellationToken ct = default);
    Task<Priority?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Priority> AddAsync(Priority entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(Priority entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}