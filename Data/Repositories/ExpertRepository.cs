using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class ExpertRepository : GenericRepository<Expert>, IExpertRepository
    {
        public ExpertRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<Expert> GetExpertByUserIdAsync(int expertId)
        {
            return await _context.Experts.FirstAsync(e => e.UserId == expertId);
        }
    }
}
