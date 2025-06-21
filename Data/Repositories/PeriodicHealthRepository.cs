using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class PeriodicHealthRepository : GenericRepository<PeriodicHealth>, IPeriodicHealthRepository
    {
        public PeriodicHealthRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<List<PeriodicHealth>> GetAllByHealthJourneyId(int healthJourneyId)
        {
            return await _context.PeriodicHealths
                .Where(ph => ph.JourneyId == healthJourneyId)
                .Include(ph => ph.Journey)
                .Include(ph => ph.Journey.Treatment)
                .ToListAsync();
        }
    }
}
