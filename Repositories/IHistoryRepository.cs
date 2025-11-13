using Web_Join.Models;

namespace Web_Join.Repositories;

public interface IHistoryRepository
{
    Task<List<History>> GetAllAsync(CancellationToken ct = default);
    Task<History?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<History> AddAsync(History entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(History entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}