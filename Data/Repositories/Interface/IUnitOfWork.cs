using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Administrator> Administrator { get; }
        IChatRoomRepository ChatRoom { get; }
        IChatRoomUserRepository ChatRoomUser { get; }
        ICommentPostRepository CommentPost { get; }
        IGenericRepository<ConnectionMedical> ConnectionMedical { get; }
        IGenericRepository<Expert> Expert { get; }
        IHealthJourneyRepository HealthJourney { get; }
        ILoginUserRepository LoginUser { get; }
        IMessageRepository Message { get; }
        IMessageAttachmentRepository MessageAttachment { get; }
        IGenericRepository<MonitoringForm> MonitoringForm { get; }
        INotificationRepository Notification { get; }
        IGenericRepository<PeriodicHealth> PeriodicHealth { get; }
        IPostRepository Post { get; }
        IGenericRepository<PostMedium> PostMedia { get; }
        IGenericRepository<PostCategory> PostCategory { get; }
        IGenericRepository<PsychologyDiary> PsychologyDiary { get; }
        IGenericRepository<Review> Review { get; }
        IGenericRepository<ReviewImage> ReviewImage { get; }
        IGenericRepository<RoleAdmin> RoleAdmin { get; }
        IGenericRepository<Treatment> Treatment { get; }
        IUserProfileRepository UserProfile { get; }
        IGenericRepository<UserType> UserType { get; }
        IPostLikeRepository PostLike { get; }

        Task<int> SaveChangesAsync(bool useTransaction = true);
    }


}
