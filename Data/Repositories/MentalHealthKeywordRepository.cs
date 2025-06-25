using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class MentalHealthKeywordRepository : GenericRepository<MentalHealthKeyword>, IMentalHealthKeyworRepository
    {
        public MentalHealthKeywordRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<(List<MentalHealthKeyword>, int)> GetAllKeyWordAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            int skip = (page - 1) * pageSize;
            var query = _context.MentalHealthKeywords
                .AsNoTracking()
                .OrderBy(k => k.KeywordId);
            int totalCount = await query.CountAsync();
            var keywords = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return (keywords, totalCount);
        }
    }
}
