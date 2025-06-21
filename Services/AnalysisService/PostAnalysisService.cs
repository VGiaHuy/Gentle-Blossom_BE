using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Services.AnalysisService
{
    // Dịch vụ điều phối phân tích bài viết và kết nối chuyên gia
    public class PostAnalysisService
    {
        private readonly KeywordAnalysisService _keywordService;// Dịch vụ phân tích từ khóa
        private readonly HuggingFaceNlpService _nlpService;// Dịch vụ gọi Hugging Face API
        private readonly GentleBlossomContext _dbContext;// Context để truy cập database
        private readonly ExpertConnectionService _expertService;// Dịch vụ kết nối chuyên gia

        public PostAnalysisService(
            KeywordAnalysisService keywordService,
            HuggingFaceNlpService nlpService,
            GentleBlossomContext dbContext,
            ExpertConnectionService expertService)
        {
            _keywordService = keywordService;
            _nlpService = nlpService;
            _dbContext = dbContext;
            _expertService = expertService;
        }

        // Hàm phân tích bài viết và quyết định có cần kết nối chuyên gia hay không
        public async Task AnalyzePostAsync(Post post)
        {
            try
            {
                // Bước 1: Phân tích Rule-based sử dụng Aho-Corasick
                var (score, matchedKeywords) = _keywordService.Analyze(post.Content);

                // Bước 2: Nếu điểm >= 15, gọi Hugging Face API để xác nhận
                if (score >= 15)
                {
                    // Tạo bản ghi phân tích ban đầu
                    var analysis = new PostAnalysis
                    {
                        PostId = post.PostId,
                        Score = score, // Điểm từ Rule-based
                        AnalysisStatus = AnalyzePost.AnalysisStatus_Pending, // Trạng thái ban đầu
                    };

                    var nlpResult = await _nlpService.AnalyzeSentiment(post.Content);
                    if (nlpResult.HasValue)
                    {
                        // Cập nhật kết quả từ Hugging Face
                        analysis.SentimentScore = nlpResult.Value.Score; // Lưu điểm số của nhãn

                        // Nếu nhãn là Negative và điểm số > 0.65, đánh dấu HIGH RISK
                        if (nlpResult.Value.Label == "negative" && nlpResult.Value.Score > 0.65)
                        {
                            analysis.RiskLevel = AnalyzePost.RiskLevel_High;
                            analysis.AnalysisStatus = AnalyzePost.AnalysisStatus_Complete;

                            // Đẩy yêu cầu kết nối vào queue để Background Service xử lý
                            await _expertService.QueueExpertConnection(post.PostId, post.PosterId, analysis.SentimentScore);
                        }
                        else
                        {
                            analysis.RiskLevel = AnalyzePost.RiskLevel_Low;
                            analysis.AnalysisStatus = AnalyzePost.AnalysisStatus_Complete;
                        }
                    }
                    else
                    {
                        // Nếu gọi Hugging Face API thất bại, đánh dấu trạng thái FAILED
                        analysis.AnalysisStatus = AnalyzePost.AnalysisStatus_Failed;
                    }

                    // Lưu kết quả phân tích vào database
                    _dbContext.PostAnalyses.Add(analysis);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi trong quá trình phân tích tâm lý bài viết: {ex.Message}");
            }
        }
    }
}
