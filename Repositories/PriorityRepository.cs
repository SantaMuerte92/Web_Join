using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class PriorityRepository : IPriorityRepository
{
    private readonly BigTicketSystemContext _ctx;
    public PriorityRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public Task<List<Priority>> GetAllAsync(CancellationToken ct = default) =>
        _ctx.Priorities.AsNoTracking().OrderBy(p => p.Title).ToListAsync(ct);

    public Task<Priority?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _ctx.Priorities.AsNoTracking().FirstOrDefaultAsync(p => p.PID == id, ct);

    public async Task<Priority> AddAsync(Priority entity, CancellationToken ct = default)
    {
        _ctx.Priorities.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Priority entity, CancellationToken ct = default)
    {
        if (!await _ctx.Priorities.AnyAsync(p => p.PID == entity.PID, ct)) return false;
        _ctx.Priorities.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Priorities.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Priorities.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}