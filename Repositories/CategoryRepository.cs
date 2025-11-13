using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly BigTicketSystemContext _ctx;
    public CategoryRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public Task<List<Category>> GetAllAsync(CancellationToken ct = default) =>
        _ctx.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(ct);

    public Task<Category?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _ctx.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CID == id, ct);

    public async Task<Category> AddAsync(Category entity, CancellationToken ct = default)
    {
        _ctx.Categories.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(Category entity, CancellationToken ct = default)
    {
        if (!await _ctx.Categories.AnyAsync(c => c.CID == entity.CID, ct)) return false;
        _ctx.Categories.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Categories.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Categories.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}