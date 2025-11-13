using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly BigTicketSystemContext _ctx;
    public AssignmentRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public Task<List<Assignment>> GetAllAsync(CancellationToken ct = default) =>
        _ctx.Assignments.AsNoTracking().ToListAsync(ct);

    public Task<Assignment?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _ctx.Assignments.AsNoTracking().FirstOrDefaultAsync(a => a.AID == id, ct);

    public async Task<Assignment> AddAsync(Assignment entity, CancellationToken ct = default)
    {
        _ctx.Assignments.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Assignment entity, CancellationToken ct = default)
    {
        if (!await _ctx.Assignments.AnyAsync(a => a.AID == entity.AID, ct)) return false;
        _ctx.Assignments.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Assignments.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Assignments.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}