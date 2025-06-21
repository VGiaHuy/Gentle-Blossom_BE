using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Administrator> Administrator { get; }
        IChatRoomRepository ChatRoom { get; }
        IChatRoomUserRepository ChatRoomUser { get; }
        ICommentPostRepository CommentPost { get; }
        IConnectionMedicalRepository ConnectionMedical { get; }
        IExpertRepository Expert { get; }
        IHealthJourneyRepository HealthJourney { get; }
        ILoginUserRepository LoginUser { get; }
        IMessageRepository Message { get; }
        IMessageAttachmentRepository MessageAttachment { get; }
        IMonitoringFormRepository MonitoringForm { get; }
        INotificationRepository Notification { get; }
        IPeriodicHealthRepository PeriodicHealth { get; }
        IPostRepository Post { get; }
        IPostMediaRepository PostMedia { get; }
        IGenericRepository<PostCategory> PostCategory { get; }
        IPsychologyDiaryRepository PsychologyDiary { get; }
        IGenericRepository<Review> Review { get; }
        IGenericRepository<ReviewImage> ReviewImage { get; }
        IGenericRepository<RoleAdmin> RoleAdmin { get; }
        IGenericRepository<Treatment> Treatment { get; }
        IUserProfileRepository UserProfile { get; }
        IGenericRepository<UserType> UserType { get; }
        IPostLikeRepository PostLike { get; }
        IPostAnalysisRepository PostAnalysis { get; }


        Task<int> SaveChangesAsync(bool useTransaction = true);
    }


}
