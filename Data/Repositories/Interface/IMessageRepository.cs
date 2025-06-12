using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<List<MessageDTO>> GetMessagesByChatRoomAsync(int chatRoomId);
        Task<Message> CreateMessageAsync(Message message);
    }
}
