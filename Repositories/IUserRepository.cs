using Web_Join.Models;

namespace Web_Join.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken ct = default);
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<User> AddAsync(User entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(User entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}