using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<(List<Notification>, int)> GetNotification(int userId, int page, int pageSize);
    }
}
