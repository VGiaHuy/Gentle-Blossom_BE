using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.DTOs.AminDTOs
{
    public class MentalHealthKeywordResponse
    {
        public List<MentalHealthKeyword> Keywords { get; set; } = new List<MentalHealthKeyword>();
        public int TotalCount { get; set; }
    }
}
