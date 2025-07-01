using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.DTOs.AminDTOs
{
    public class ExpertResponse
    {
        public List<ExpertProfileDTO> Experts { get; set; } = new List<ExpertProfileDTO>();
        public int TotalCount { get; set; }
    }
}
