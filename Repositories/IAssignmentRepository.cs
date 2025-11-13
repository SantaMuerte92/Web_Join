using Web_Join.Models;

namespace Web_Join.Repositories;

public interface IAssignmentRepository
{
    Task<List<Assignment>> GetAllAsync(CancellationToken ct = default);
    Task<Assignment?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Assignment> AddAsync(Assignment entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(Assignment entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}