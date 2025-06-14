using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IPostAnalysisRepository : IGenericRepository<PostAnalysis>
    {
        Task<bool> DeleteByPostId(int postId);
    }
}
