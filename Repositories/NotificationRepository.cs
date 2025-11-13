using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly BigTicketSystemContext _ctx;
    public NotificationRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public Task<List<Notification>> GetAllAsync(CancellationToken ct = default) =>
        _ctx.Notifications.AsNoTracking().OrderByDescending(n => n.CreatedAt).ToListAsync(ct);

    public Task<Notification?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _ctx.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.NID == id, ct);

    public async Task<Notification> AddAsync(Notification entity, CancellationToken ct = default)
    {
        _ctx.Notifications.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Notification entity, CancellationToken ct = default)
    {
        if (!await _ctx.Notifications.AnyAsync(n => n.NID == entity.NID, ct)) return false;
        _ctx.Notifications.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Notifications.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Notifications.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}