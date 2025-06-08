using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class ChatRoomRepository : GenericRepository<ChatRoom>, IChatRoomRepository
    {
        public ChatRoomRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom)
        {
            await _context.ChatRooms.AddAsync(chatRoom);
            await _context.SaveChangesAsync();
            return chatRoom;
        }

        public async Task<List<ChatRoom>> GetChatRoomsByUserAsync(int userId)
        {
            return await _context.ChatRoomUsers
                .Where(cru => cru.ParticipantId == userId)
                .Join(_context.ChatRooms,
                    cru => cru.ChatRoomId,
                    cr => cr.ChatRoomId,
                    (cru, cr) => cr)
                .ToListAsync();
        }
    }
}
