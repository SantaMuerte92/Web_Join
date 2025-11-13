using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class HistoryRepository : IHistoryRepository
{
    private readonly BigTicketSystemContext _ctx;
    public HistoryRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public Task<List<History>> GetAllAsync(CancellationToken ct = default) =>
        _ctx.Histories.AsNoTracking().OrderByDescending(h => h.Modificationdate).ToListAsync(ct);

    public Task<History?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _ctx.Histories.AsNoTracking().FirstOrDefaultAsync(h => h.HID == id, ct);

    public async Task<History> AddAsync(History entity, CancellationToken ct = default)
    {
        _ctx.Histories.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(History entity, CancellationToken ct = default)
    {
        if (!await _ctx.Histories.AnyAsync(h => h.HID == entity.HID, ct)) return false;
        _ctx.Histories.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Histories.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Histories.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}