using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IChatRoomRepository : IGenericRepository<ChatRoom>
    {
        Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom);
        Task<List<ChatRoom>> GetChatRoomsByUserAsync(int userId);
    }
}
