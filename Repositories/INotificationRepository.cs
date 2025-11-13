using Web_Join.Models;

namespace Web_Join.Repositories;

public interface INotificationRepository
{
    Task<List<Notification>> GetAllAsync(CancellationToken ct = default);
    Task<Notification?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Notification> AddAsync(Notification entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(Notification entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}