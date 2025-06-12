using GentleBlossom_BE.Data.DTOs.UserDTOs;
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

        public async Task<List<MessageDTO>> GetMessagesByChatRoomAsync(int chatRoomId)
        {
            return await _context.Messages
                .Where(m => m.ChatRoomId == chatRoomId)
                .Include(m => m.Sender)
                .Include(m => m.MessageAttachments)
                .OrderBy(m => m.SentAt)
                .Select(m => new MessageDTO
                {
                    MessageId = m.MessageId,
                    ChatRoomId = m.ChatRoomId,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.FullName,
                    AvatarUrl = m.Sender.AvatarUrl,
                    Content = m.Content,
                    MediaList = m.MessageAttachments.Select(media => new MessageMediaDTO
                    {
                        MediaUrl = media.FileUrl,
                        MediaType = media.FileType,
                        FileName = media.FileName
                    }).ToList(),
                    SentAt = m.SentAt ?? default,
                    IsDeleted = m.IsDeleted
                })
                .ToListAsync();
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            return message;
        }
    }
}
