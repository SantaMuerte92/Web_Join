using Web_Join.Models;

namespace Web_Join.Repositories;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync(CancellationToken ct = default);
    Task<Comment?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Comment> AddAsync(Comment entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(Comment entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}