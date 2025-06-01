using GentleBlossom_BE.Data.Responses;
using System.Text;
using System.Text.Json;

namespace GentleBlossom_BE.Services.AnalysisService
{
    // Dịch vụ gọi Hugging Face API để phân tích cảm xúc
    public class HuggingFaceNlpService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://api-inference.huggingface.co/models/cardiffnlp/twitter-xlm-roberta-base-sentiment";

        // Constructor, nhận HttpClient và API Key từ cấu hình
        public HuggingFaceNlpService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["HuggingFace:ApiKey"];
            // Thêm API Key vào header cho mọi yêu cầu
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        // Phân tích cảm xúc của nội dung bài viết
        public async Task<(string Label, float Score)?> AnalyzeSentiment(string content)
        {
            // Kiểm tra nội dung đầu vào
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            // Tạo body yêu cầu cho Hugging Face API
            var requestBody = new
            {
                inputs = content
            };

            try
            {
                // Gửi yêu cầu POST tới Hugging Face API
                var response = await _httpClient.PostAsync(
                    _apiUrl,
                    new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

                // Kiểm tra nếu yêu cầu thất bại
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                // Đọc nội dung JSON từ phản hồi
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialize JSON thành mảng lồng mảng
                var result = await response.Content.ReadFromJsonAsync<HuggingFaceResponse[][]>();
                if (result == null || result.Length == 0 || result[0].Length == 0)
                {
                    return null;
                }

                // Tìm nhãn có điểm số cao nhất
                var topResult = result[0].OrderByDescending(r => r.Score).First();
                return (topResult.Label, topResult.Score);
            }
            catch (JsonException ex)
            {
                // Log lỗi khi deserialize thất bại
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Log lỗi tổng quát
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }
    }
}
