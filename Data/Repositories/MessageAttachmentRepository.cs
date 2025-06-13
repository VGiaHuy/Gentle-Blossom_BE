using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class MessageAttachmentRepository : GenericRepository<MessageAttachment>, IMessageAttachmentRepository
    {
        public MessageAttachmentRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<bool> DeleteByMessageId(int messageId)
        {
            try
            {
                List<MessageAttachment> messages = await _context.MessageAttachments
                    .Where(a => a.MessageId == messageId)
                    .ToListAsync();

                if (messages == null || !messages.Any())
                {
                    return true;
                }

                _context.MessageAttachments.RemoveRange(messages);

                int rowsAffected = await _context.SaveChangesAsync();

                return rowsAffected > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
