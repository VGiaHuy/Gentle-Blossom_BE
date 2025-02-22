using GentleBlossom_BE.Data.Models;
using System;

namespace GentleBlossom_BE.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GentleBlossomContext _context;

        public IGenericRepository<Administrator> Administrator { get; }
        public IGenericRepository<ChatRoom> ChatRoom { get; }
        public IGenericRepository<ChatRoomUser> ChatRoomUser { get; }
        public IGenericRepository<CommentPost> CommentPost { get; }
        public IGenericRepository<ConnectionMedical> ConnectionMedical { get; }
        public IGenericRepository<Expert> Expert { get; }
        public IGenericRepository<HealthJourney> HealthJourney { get; }
        public IGenericRepository<LoginUser> LoginUser { get; }
        public IGenericRepository<Message> Message { get; }
        public IGenericRepository<MessageAttachment> MessageAttachment { get; }
        public IGenericRepository<MonitoringForm> MonitoringForm { get; }
        public IGenericRepository<Notification> Notification { get; }
        public IGenericRepository<PeriodicHealth> PeriodicHealth { get; }
        public IGenericRepository<Post> Post { get; }
        public IGenericRepository<PostImage> PostImage { get; }
        public IGenericRepository<PostCategory> PostCategory { get; }
        public IGenericRepository<PsychologyDiary> PsychologyDiary { get; }
        public IGenericRepository<Review> Review { get; }
        public IGenericRepository<ReviewImage> ReviewImage { get; }
        public IGenericRepository<RoleAdmin> RoleAdmin { get; }
        public IGenericRepository<Treatment> Treatment { get; }
        public IGenericRepository<UserProfile> UserProfile { get; }
        public IGenericRepository<UserType> UserType { get; }

        public UnitOfWork(GentleBlossomContext context)
        {
            _context = context;
            Administrator = new GenericRepository<Administrator>(_context);
            ChatRoom = new GenericRepository<ChatRoom>(_context);
            ChatRoomUser = new GenericRepository<ChatRoomUser>(_context);
            CommentPost = new GenericRepository<CommentPost>(_context);
            ConnectionMedical = new GenericRepository<ConnectionMedical>(_context);
            Expert = new GenericRepository<Expert>(_context);
            HealthJourney = new GenericRepository<HealthJourney>(_context);
            LoginUser = new GenericRepository<LoginUser>(_context);
            Message = new GenericRepository<Message>(_context);
            MessageAttachment = new GenericRepository<MessageAttachment>(_context);
            MonitoringForm = new GenericRepository<MonitoringForm>(_context);
            Notification = new GenericRepository<Notification>(_context);
            PeriodicHealth = new GenericRepository<PeriodicHealth>(_context);
            Post = new GenericRepository<Post>(_context);
            PostImage = new GenericRepository<PostImage>(_context);
            PostCategory = new GenericRepository<PostCategory>(_context);
            PsychologyDiary = new GenericRepository<PsychologyDiary>(_context);
            Review = new GenericRepository<Review>(_context);
            ReviewImage = new GenericRepository<ReviewImage>(_context);
            RoleAdmin = new GenericRepository<RoleAdmin>(_context);
            Treatment = new GenericRepository<Treatment>(_context);
            UserProfile = new GenericRepository<UserProfile>(_context);
            UserType = new GenericRepository<UserType>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }


}
