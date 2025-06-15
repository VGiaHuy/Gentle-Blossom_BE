using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IConnectionMedicalRepository : IGenericRepository<ConnectionMedical>
    {
        Task<bool> DeleteByPostId(int postId);
    }
}
