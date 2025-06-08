using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<List<Message>> GetMessagesByChatRoomAsync(int chatRoomId)
        {
            return await _context.Messages
                .Where(m => m.ChatRoomId == chatRoomId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            return message;
        }
    }
}
