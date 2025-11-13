using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly BigTicketSystemContext _ctx;
    public AttachmentRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public Task<List<Attachment>> GetAllAsync(CancellationToken ct = default) =>
        _ctx.Attachments.AsNoTracking().OrderByDescending(a => a.Uploadtime).ToListAsync(ct);

    public Task<Attachment?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _ctx.Attachments.AsNoTracking().FirstOrDefaultAsync(a => a.ATID == id, ct);

    public async Task<Attachment> AddAsync(Attachment entity, CancellationToken ct = default)
    {
        _ctx.Attachments.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Attachment entity, CancellationToken ct = default)
    {
        if (!await _ctx.Attachments.AnyAsync(a => a.ATID == entity.ATID, ct)) return false;
        _ctx.Attachments.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Attachments.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Attachments.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}