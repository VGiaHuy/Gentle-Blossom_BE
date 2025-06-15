using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using System;

namespace GentleBlossom_BE.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GentleBlossomContext _context;

        public IGenericRepository<Administrator> Administrator { get; }
        public IChatRoomRepository ChatRoom { get; }
        public IChatRoomUserRepository ChatRoomUser { get; }
        public ICommentPostRepository CommentPost { get; }
        public IConnectionMedicalRepository ConnectionMedical { get; }
        public IGenericRepository<Expert> Expert { get; }
        public IHealthJourneyRepository HealthJourney { get; }
        public ILoginUserRepository LoginUser { get; }
        public IMessageRepository Message { get; }
        public IMessageAttachmentRepository MessageAttachment { get; }
        public IGenericRepository<MonitoringForm> MonitoringForm { get; }
        public INotificationRepository Notification { get; }
        public IGenericRepository<PeriodicHealth> PeriodicHealth { get; }
        public IPostRepository Post { get; }
        public IPostMediaRepository PostMedia { get; }
        public IGenericRepository<PostCategory> PostCategory { get; }
        public IGenericRepository<PsychologyDiary> PsychologyDiary { get; }
        public IGenericRepository<Review> Review { get; }
        public IGenericRepository<ReviewImage> ReviewImage { get; }
        public IGenericRepository<RoleAdmin> RoleAdmin { get; }
        public IGenericRepository<Treatment> Treatment { get; }
        public IUserProfileRepository UserProfile { get; }
        public IGenericRepository<UserType> UserType { get; }
        public IPostLikeRepository PostLike { get;}
        public IPostAnalysisRepository PostAnalysis { get; }

        public UnitOfWork(GentleBlossomContext context)
        {
            _context = context;
            Administrator = new GenericRepository<Administrator>(_context);
            ChatRoom = new ChatRoomRepository(_context);
            ChatRoomUser = new ChatRoomUserRepository(_context);
            CommentPost = new CommentPostRepository(_context);
            ConnectionMedical = new ConnectionMedicalRepository(_context);
            Expert = new GenericRepository<Expert>(_context);
            HealthJourney = new HealthJourneyRepository(_context);
            LoginUser = new LoginUserRepository(_context);
            Message = new MessageRepository(_context);
            MessageAttachment = new MessageAttachmentRepository(_context);
            MonitoringForm = new GenericRepository<MonitoringForm>(_context);
            Notification = new NotificationRepository(_context);
            PeriodicHealth = new GenericRepository<PeriodicHealth>(_context);
            Post = new PostRepository(_context);
            PostMedia = new PostMediaRepository(_context);
            PostCategory = new GenericRepository<PostCategory>(_context);
            PsychologyDiary = new GenericRepository<PsychologyDiary>(_context);
            Review = new GenericRepository<Review>(_context);
            ReviewImage = new GenericRepository<ReviewImage>(_context);
            RoleAdmin = new GenericRepository<RoleAdmin>(_context);
            Treatment = new GenericRepository<Treatment>(_context);
            UserProfile = new UserProfileRepository(_context);
            UserType = new GenericRepository<UserType>(_context);
            PostLike = new PostLikeRepository(_context);
            PostAnalysis = new PostAnalysisRepository(_context);
        }

        public async Task<int> SaveChangesAsync(bool useTransaction = true)
        {
            if (useTransaction)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new InternalServerException("Đã xảy ra lỗi khi lưu dữ liệu: " + ex);
                }
            }
            else
            {
                try
                {
                    return await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new InternalServerException("Đã xảy ra lỗi khi lưu dữ liệu: " + ex);
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose(); // Giải phóng tài nguyên của DbContext
        }
    }
}
