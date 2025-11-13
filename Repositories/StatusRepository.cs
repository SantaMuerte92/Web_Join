using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly BigTicketSystemContext _ctx;
    public StatusRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public Task<List<Status>> GetAllAsync(CancellationToken ct = default) =>
        _ctx.Statuses.AsNoTracking().OrderBy(s => s.Title).ToListAsync(ct);

    public Task<Status?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _ctx.Statuses.AsNoTracking().FirstOrDefaultAsync(s => s.SID == id, ct);

    public async Task<Status> AddAsync(Status entity, CancellationToken ct = default)
    {
        _ctx.Statuses.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Status entity, CancellationToken ct = default)
    {
        if (!await _ctx.Statuses.AnyAsync(s => s.SID == entity.SID, ct)) return false;
        _ctx.Statuses.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Statuses.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Statuses.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}