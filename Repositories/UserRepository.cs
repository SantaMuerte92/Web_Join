using Microsoft.EntityFrameworkCore;
using Web_Join.Models;

namespace Web_Join.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BigTicketSystemContext _ctx;
    public UserRepository(BigTicketSystemContext ctx) => _ctx = ctx;

    public Task<List<User>> GetAllAsync(CancellationToken ct = default) =>
        _ctx.Users.AsNoTracking().OrderBy(u => u.Name).ToListAsync(ct);

    public Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _ctx.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UID == id, ct);

    public async Task<User> AddAsync(User entity, CancellationToken ct = default)
    {
        _ctx.Users.Add(entity);
        await _ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateAsync(User entity, CancellationToken ct = default)
    {
        if (!await _ctx.Users.AnyAsync(u => u.UID == entity.UID, ct)) return false;
        _ctx.Users.Update(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _ctx.Users.FindAsync(new object[] { id }, ct);
        if (entity == null) return false;
        _ctx.Users.Remove(entity);
        return await _ctx.SaveChangesAsync(ct) > 0;
    }
}