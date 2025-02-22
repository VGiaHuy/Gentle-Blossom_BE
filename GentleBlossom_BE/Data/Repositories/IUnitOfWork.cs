using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Administrator> Administrator { get; }
        IGenericRepository<ChatRoom> ChatRoom { get; }
        IGenericRepository<ChatRoomUser> ChatRoomUser { get; }
        IGenericRepository<CommentPost> CommentPost { get; }
        IGenericRepository<ConnectionMedical> ConnectionMedical { get; }
        IGenericRepository<Expert> Expert { get; }
        IGenericRepository<HealthJourney> HealthJourney { get; }
        IGenericRepository<LoginUser> LoginUser { get; }
        IGenericRepository<Message> Message { get; }
        IGenericRepository<MessageAttachment> MessageAttachment { get; }
        IGenericRepository<MonitoringForm> MonitoringForm { get; }
        IGenericRepository<Notification> Notification { get; }
        IGenericRepository<PeriodicHealth> PeriodicHealth { get; }
        IGenericRepository<Post> Post { get; }
        IGenericRepository<PostImage> PostImage { get; }
        IGenericRepository<PostCategory> PostCategory { get; }
        IGenericRepository<PsychologyDiary> PsychologyDiary { get; }
        IGenericRepository<Review> Review { get; }
        IGenericRepository<ReviewImage> ReviewImage { get; }
        IGenericRepository<RoleAdmin> RoleAdmin { get; }
        IGenericRepository<Treatment> Treatment { get; }
        IGenericRepository<UserProfile> UserProfile { get; }
        IGenericRepository<UserType> UserType { get; }


        Task<int> SaveChangesAsync();
    }


}
