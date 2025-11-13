using Web_Join.Models;

namespace Web_Join.Repositories;

public interface IAttachmentRepository
{
    Task<List<Attachment>> GetAllAsync(CancellationToken ct = default);
    Task<Attachment?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Attachment> AddAsync(Attachment entity, CancellationToken ct = default);
    Task<bool> UpdateAsync(Attachment entity, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}