using Web_Join.Models;

namespace Web_Join.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CancellationToken ct = default);
    Task<Category?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Category> AddAsync(Category entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(Category entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}