using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Administrator> Administrator { get; }
        IGenericRepository<ChatRoom> ChatRoom { get; }
        IGenericRepository<ChatRoomUser> ChatRoomUser { get; }
        ICommentPostRepository CommentPost { get; }
        IGenericRepository<ConnectionMedical> ConnectionMedical { get; }
        IGenericRepository<Expert> Expert { get; }
        IGenericRepository<HealthJourney> HealthJourney { get; }
        ILoginUserRepository LoginUser { get; }
        IGenericRepository<Message> Message { get; }
        IGenericRepository<MessageAttachment> MessageAttachment { get; }
        IGenericRepository<MonitoringForm> MonitoringForm { get; }
        IGenericRepository<Notification> Notification { get; }
        IGenericRepository<PeriodicHealth> PeriodicHealth { get; }
        IPostRepository Post { get; }
        IGenericRepository<PostImage> PostImage { get; }
        IGenericRepository<PostCategory> PostCategory { get; }
        IGenericRepository<PsychologyDiary> PsychologyDiary { get; }
        IGenericRepository<Review> Review { get; }
        IGenericRepository<ReviewImage> ReviewImage { get; }
        IGenericRepository<RoleAdmin> RoleAdmin { get; }
        IGenericRepository<Treatment> Treatment { get; }
        IUserProfileRepository UserProfile { get; }
        IGenericRepository<UserType> UserType { get; }

        Task<int> SaveChangesAsync(bool useTransaction = true);
    }


}
