using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IExpertRepository : IGenericRepository<Expert>
    {
        Task<Expert> GetExpertByUserIdAsync(int expertId);
    }
}
