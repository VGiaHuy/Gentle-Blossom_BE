using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Infrastructure;

namespace GentleBlossom_BE.Services.AnalysisService
{
    public class KeywordAnalysisService
    {
        private readonly AhoCorasickTrie _trie;
        private readonly List<MentalHealthKeyword> _keywords;

        // Dịch vụ phân tích Rule-based dựa trên từ khóa tâm lý
        public KeywordAnalysisService(IConfiguration configuration, GentleBlossomContext dbContext)
        {
            // Lấy danh sách từ khóa đang hoạt động từ database
            _keywords = dbContext.MentalHealthKeywords
                .Where(k => (bool)k.IsActive!) // Lọc từ khóa có IsActive = true
                .ToList();

            _trie = new AhoCorasickTrie();
            // Thêm từng từ khóa vào trie
            foreach (var keyword in _keywords)
            {
                _trie.Add(keyword.Keyword.ToLower(), keyword); // Lưu MentalHealthKeyword vào trie
            }
            _trie.Build(); // Xây dựng trie với liên kết thất bại
        }

        // Phân tích nội dung bài viết, trả về điểm rủi ro và danh sách từ khóa khớp
        public (int Score, List<MentalHealthKeyword> MatchedKeywords) Analyze(string content)
        {
            // Tìm kiếm từ khóa trong nội dung (chuyển thành lowercase để không phân biệt hoa thường)
            var matches = _trie.Search(content.ToLower());
            int score = 0;
            var matchedKeywords = new List<MentalHealthKeyword>();

            // Duyệt qua các từ khóa khớp được trả về từ trie
            foreach (var match in matches)
            {
                // Kiểm tra nếu match là MentalHealthKeyword để đảm bảo an toàn
                if (match is MentalHealthKeyword keyword)
                {
                    score += keyword.Weight; // Cộng trọng số của từ khóa vào điểm tổng
                    matchedKeywords.Add(keyword); // Thêm từ khóa vào danh sách khớp
                }
            }

            return (score, matchedKeywords); // Trả về điểm và danh sách từ khóa khớp
        }
    }
}
