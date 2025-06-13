using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IMessageAttachmentRepository : IGenericRepository<MessageAttachment>
    {
        Task<bool> DeleteByMessageId(int messageId);
    }
}
