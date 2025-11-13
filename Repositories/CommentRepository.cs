using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly BigTicketSystemContext _ctx;
    public CommentRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public Task<List<Comment>> GetAllAsync(CancellationToken ct = default) =>
        _ctx.Comments.AsNoTracking().OrderByDescending(c => c.Creatredate).ToListAsync(ct);

    public Task<Comment?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _ctx.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.COID == id, ct);

    public async Task<Comment> AddAsync(Comment entity, CancellationToken ct = default)
    {
        _ctx.Comments.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Comment entity, CancellationToken ct = default)
    {
        if (!await _ctx.Comments.AnyAsync(c => c.COID == entity.COID, ct)) return false;
        _ctx.Comments.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Comments.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Comments.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}