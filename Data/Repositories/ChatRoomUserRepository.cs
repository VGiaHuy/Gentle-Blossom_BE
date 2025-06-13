using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class ChatRoomUserRepository : GenericRepository<ChatRoomUser>, IChatRoomUserRepository
    {
        public ChatRoomUserRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task RemoveUserFromChatRoomAsync(int chatRoomId, int participantId)
        {
            var chatRoomUser = await _context.ChatRoomUsers
                .FirstOrDefaultAsync(cru => cru.ChatRoomId == chatRoomId && cru.ParticipantId == participantId);
            if (chatRoomUser != null)
            {
                _context.ChatRoomUsers.Remove(chatRoomUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ChatRoomUser>> GetChatRoomUsersAsync(int userId)
        {
            return await _context.ChatRoomUsers
                .Where(cru => cru.ParticipantId == userId)
                .ToListAsync();
        }

        public Task<bool> CheckUserExistInChatRoom(int chatRoomId, int userId)
        {
            return _context.ChatRoomUsers.Where(a => a.ParticipantId == userId && a.ChatRoomId == chatRoomId).AnyAsync();
        }
    }
}
