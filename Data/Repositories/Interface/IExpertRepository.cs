using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IExpertRepository : IGenericRepository<Expert>
    {
        Task<Expert> GetExpertByUserIdAsync(int expertId);
        Task<List<ExpertProfileDTO>> GetAllExperts();
        Task<ExpertProfileDTO> GetExpertsProfile(int expertId);
        Task<(List<ExpertProfileDTO>, int)> GetAllExpertsAsync(int page, int pageSize);
    }
}
