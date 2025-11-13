using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly BigTicketSystemContext _ctx;
    public TicketRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public async Task<List<Ticket>> GetAllAsync(bool onlyOpen = false, CancellationToken ct = default)
    {
        var q = _ctx.Tickets
            .Include(t => t.User)
            .Include(t => t.Status)
            .Include(t => t.Priority)
            .Include(t => t.Category)
            .AsQueryable();

        if (onlyOpen)
            q = q.Where(t => t.Status.Title != "Geschlossen" && t.Status.Title != "Gelöst");

        return await q.AsNoTracking()
            .OrderByDescending(t => t.Createdate)
            .ToListAsync(ct);
    }

    public async Task<Ticket?> GetByIdAsync(int id, bool includeRelated = true, CancellationToken ct = default)
    {
        IQueryable<Ticket> q = _ctx.Tickets;
        if (includeRelated)
        {
            q = q
                .Include(t => t.User)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.Category)
                .Include(t => t.Attachments)
                .Include(t => t.Comments)
                .Include(t => t.Histories)
                .Include(t => t.Notifications);
        }
        return await q.AsNoTracking().FirstOrDefaultAsync(t => t.TID == id, ct);
    }

    public async Task<Ticket> AddAsync(Ticket entity, CancellationToken ct = default)
    {
        _ctx.Tickets.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Ticket entity, CancellationToken ct = default)
    {
        if (!await _ctx.Tickets.AnyAsync(t => t.TID == entity.TID, ct)) return false;
        _ctx.Tickets.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Tickets.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Tickets.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}