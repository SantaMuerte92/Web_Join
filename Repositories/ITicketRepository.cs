using Web_Join.Models;

namespace Web_Join.Repositories;

public interface ITicketRepository
{
    Task<List<Ticket>> GetAllAsync(bool onlyOpen = false, CancellationToken ct = default);
    Task<Ticket?> GetByIdAsync(int id, bool includeRelated = true, CancellationToken ct = default);
    Task<Ticket> AddAsync(Ticket entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(Ticket entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}