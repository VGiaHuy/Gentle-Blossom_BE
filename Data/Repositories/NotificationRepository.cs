using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<(List<Notification>, int)> GetNotification(int userId, int page, int pageSize)
        {
            var query = _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreateAt);

            // Lấy tổng số thông báo để hiển thị badge
            int totalCount = await query.Where(n => n.IsSeen == false).CountAsync();

            // Lấy danh sách thông báo phân trang
            var notifications = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (notifications, totalCount);
        }
    }
}
