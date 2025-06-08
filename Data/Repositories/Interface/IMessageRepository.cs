using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<List<Message>> GetMessagesByChatRoomAsync(int chatRoomId);
        Task<Message> CreateMessageAsync(Message message);
    }
}
