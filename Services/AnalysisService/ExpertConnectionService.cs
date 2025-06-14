using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Services.AnalysisService
{
    // Lớp lưu trữ thông tin yêu cầu kết nối (PostId)
    public class ExpertConnectionRequest
    {
        public int PostId { get; set; }
        public int PosterId { get; set; }
    }

    public class ExpertConnectionService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly InMemoryQueue<ExpertConnectionRequest> _queue;// Hàng đợi lưu trữ yêu cầu

        public ExpertConnectionService(IServiceProvider serviceProvider, InMemoryQueue<ExpertConnectionRequest> queue)
        {
            _serviceProvider = serviceProvider;
            _queue = queue;
        }

        // Hàm để đẩy yêu cầu kết nối vào hàng đợi
        public async Task QueueExpertConnection(int postId, int posterId)
        {
            // Tạo yêu cầu mới với PostId
            var request = new ExpertConnectionRequest { PostId = postId, PosterId = posterId };
            // Đẩy yêu cầu vào queue
            await _queue.EnqueueAsync(request);
        }

        // Hàm chính của Background Service, chạy liên tục để xử lý queue
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Vòng lặp chạy cho đến khi ứng dụng dừng
            while (!stoppingToken.IsCancellationRequested)
            {
                // Chờ và lấy yêu cầu từ queue
                var request = await _queue.DequeueAsync(stoppingToken);

                // Tạo scope mới để sử dụng DbContext (tránh lỗi lifetime)
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<GentleBlossomContext>();

                // Tìm chuyên gia khả dụng (Chuyên môn là Tâm lý học)
                var expert = await dbContext.Experts
                    .Where(e => e.Specialization == SpecializationExpert.Psychology)
                    .FirstOrDefaultAsync(stoppingToken);

                if (expert != null)
                {
                    var expertInfo = await dbContext.UserProfiles
                        .Where(u => u.UserId == expert.UserId)
                        .Select(e => e.FullName)
                        .FirstOrDefaultAsync(stoppingToken);

                    var userInfo = await dbContext.UserProfiles
                        .Where(u => u.UserId == request.PosterId)
                        .Select(u => u.FullName)
                        .FirstOrDefaultAsync(stoppingToken);

                    // Tạo bản ghi kết nối trong bảng ExpertConnections
                    var connection = new ConnectionMedical
                    {
                        PostId = request.PostId,
                        UserId = request.PosterId,
                        ExpertId = expert.ExpertId,
                        CreatedAt = DateTime.UtcNow
                    };

                    var notificationUser = new Notification
                    {
                        UserId = request.PosterId,
                        Content = $"Chúng tôi nhận thấy bạn đang có vấn đề về tâm lý. Chuyên gia {expert.Specialization} {expert.AcademicTitle} {expertInfo} sẽ được kết nối đến bạn.",
                        CreateAt = DateTime.UtcNow,
                        Url = $"/PregnancyCare/ConnectPost/{request.PostId}" // Đường dẫn đến bài viết
                    };

                    var notificationExpert = new Notification
                    {
                        UserId = expert.UserId,
                        Content = $"PHÁT HIỆN BỆNH NHÂN!!! Bạn đang được kết nối với người dùng {userInfo} đang có dâu hiệu tâm lý không ổn định",
                        CreateAt = DateTime.UtcNow,
                        Url = $"/PregnancyCare/ConnectPost/{request.PostId}" // Đường dẫn đến bài viết
                        
                    };

                    // Lưu vào database
                    dbContext.ConnectionMedicals.Add(connection);
                    dbContext.Notifications.Add(notificationUser);
                    dbContext.Notifications.Add(notificationExpert);
                    await dbContext.SaveChangesAsync();

                    // Gửi thông báo đến chuyên gia (giả lập hoặc tích hợp email/SMS)
                    await NotifyExpertAsync(expert, request.PostId);
                }
                else
                {
                    // Log trường hợp không tìm thấy chuyên gia (có thể thêm retry logic)
                    Console.WriteLine($"No available expert for post {request.PostId}");
                }
            }
        }

        // Hàm gửi thông báo đến chuyên gia (giả lập, có thể mở rộng)
        private async Task NotifyExpertAsync(Expert expert, long postId)
        {
            // Giả lập gửi thông báo (thay bằng email/SMS thực tế nếu cần)
            await Task.CompletedTask; // Đảm bảo async
        }
    }
}
