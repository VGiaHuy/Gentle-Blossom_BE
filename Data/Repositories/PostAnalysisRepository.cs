using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class PostAnalysisRepository : GenericRepository<PostAnalysis>, IPostAnalysisRepository
    {
        public PostAnalysisRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<bool> DeleteByPostId(int postId)
        {
            var postAnalysis = await _context.PostAnalyses.FirstOrDefaultAsync(pa => pa.PostId == postId);
            if (postAnalysis != null)
            {
                _context.PostAnalyses.Remove(postAnalysis);
                return true;
            }
            return false;
        }
    }
}
