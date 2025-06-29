using GentleBlossom_BE.Data.DTOs.UserDTOs;
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

        public Task<int> GetUseIdByChatRoomIdAsync(int chatRoomId, int expertId)
        {
            return _context.ChatRoomUsers
                .Where(cru => cru.ChatRoomId == chatRoomId && cru.ParticipantId != expertId)
                .Select(cru => cru.ParticipantId)
                .FirstAsync();
        }

        public async Task<bool> AreParticipantsInPrivateChatRoomAsync(int participantId1, int participantId2, CancellationToken cancellationToken = default)
        {
            var hasPrivateChatRoom = await _context.ChatRoomUsers
                .AsNoTracking()
                .Where(cru => cru.ParticipantId == participantId1)
                .Join(_context.ChatRoomUsers
                        .Where(cru => cru.ParticipantId == participantId2),
                      cru1 => cru1.ChatRoomId,
                      cru2 => cru2.ChatRoomId,
                      (cru1, cru2) => cru1.ChatRoomId)
                .Join(_context.ChatRooms
                        .Where(cr => cr.IsGroup == false),
                      cru => cru,
                      cr => cr.ChatRoomId,
                      (cru, cr) => cr.ChatRoomId)
                .AnyAsync(cancellationToken);

            return hasPrivateChatRoom;
        }

        public async Task<List<UsersInChatRoomDTO>> GetUsersInChatRoom(int chatRoomId)
        {
            return await _context.ChatRoomUsers
                .AsNoTracking()
                .Where(cru => cru.ChatRoomId == chatRoomId)
                .Select(users => new UsersInChatRoomDTO
                {
                    Id = users.ParticipantId,
                    Name = users.Participant.FullName,
                    ConnectionId = null // Sẽ được điền bởi service
                })
                .ToListAsync();
        }

        public async Task<int?> GetExistingPrivateChatRoomIdAsync(int userId1, int userId2)
        {
            // Lấy danh sách các ChatRoomUser có liên quan 2 người
            var chatRoomUsers = await _context.ChatRoomUsers
                .Where(c => c.ParticipantId == userId1 || c.ParticipantId == userId2)
                .ToListAsync();

            // Lấy danh sách các phòng có 2 người dùng cùng tham gia
            var chatRoomIds = chatRoomUsers
                .GroupBy(c => c.ChatRoomId)
                .Where(g => g.Count() == 2)
                .Select(g => g.Key)
                .ToList();

            // Lấy ra phòng có isGroup = false
            var privateRoomId = await _context.ChatRooms
                .Where(r => chatRoomIds.Contains(r.ChatRoomId) && !r.IsGroup)
                .Select(r => r.ChatRoomId)
                .FirstOrDefaultAsync();

            return privateRoomId == 0 ? (int?)null : privateRoomId;
        }

    }
}
