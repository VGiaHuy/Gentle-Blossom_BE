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

        public double? SentimentScore { get; set; }
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
        public async Task QueueExpertConnection(int postId, int posterId, double? sentimentScore)
        {
            var request = new ExpertConnectionRequest { PostId = postId, PosterId = posterId, SentimentScore = sentimentScore };
            await _queue.EnqueueAsync(request);
        }

        // Hàm chính của Background Service, chạy liên tục để xử lý queue
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var request = await _queue.DequeueAsync(stoppingToken);
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<GentleBlossomContext>();
                    Expert expert;

                    // Tìm kiếm người dùng có đang trong quá trình điều trị nào không?
                    var checkConnectionExists = await dbContext.ConnectionMedicals
                            .Where(cm => cm.UserId == request.PosterId && cm.Status == 0)
                            .FirstOrDefaultAsync();

                    if (checkConnectionExists != null)
                    {
                        expert = await dbContext.Experts
                            .Where(e => e.ExpertId == checkConnectionExists.ExpertId)
                            .FirstAsync();
                    }
                    else
                    {
                        expert = await dbContext.Experts
                            .AsNoTracking()
                            .Where(e => e.Specialization == SpecializationExpert.Psychology)
                            .OrderBy(e => Guid.NewGuid())
                            .FirstAsync(stoppingToken);
                    }

                    // lấy ra thông tin chuyên gia và thai phụ
                    var expertInfo = await dbContext.UserProfiles
                        .Where(u => u.UserId == expert.UserId)
                        .Select(e => e.FullName)
                        .FirstOrDefaultAsync(stoppingToken);

                    var userInfo = await dbContext.UserProfiles
                        .Where(u => u.UserId == request.PosterId)
                        .Select(u => u.FullName)
                        .FirstOrDefaultAsync(stoppingToken);

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
                        Url = $"/PregnancyCare/ConnectPost/{request.PostId}"
                    };

                    var notificationExpert = new Notification
                    {
                        UserId = expert.UserId,
                        Content = $"PHÁT HIỆN BỆNH NHÂN!!! Bạn đang được kết nối với người dùng {userInfo} đang có dâu hiệu tâm lý không ổn định",
                        CreateAt = DateTime.UtcNow,
                        Url = $"/PregnancyCare/ConnectPost/{request.PostId}"

                    };

                    dbContext.ConnectionMedicals.Add(connection);
                    dbContext.Notifications.Add(notificationUser);
                    dbContext.Notifications.Add(notificationExpert);
                    await dbContext.SaveChangesAsync();

                    // Gửi thông báo đến chuyên gia (giả lập hoặc tích hợp email/SMS)
                    await NotifyExpertAsync(expert, request.PostId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi xử lý ExpertConnectionRequest" + ex);
                }
            }
        }

        private async Task NotifyExpertAsync(Expert expert, long postId)
        {

            await Task.CompletedTask;
        }
    }
}
