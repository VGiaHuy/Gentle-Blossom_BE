using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class HealthJourneyRepository : GenericRepository<HealthJourney>, IHealthJourneyRepository
    {
        public HealthJourneyRepository(Models.GentleBlossomContext context) : base(context)
        {
        }

        public async Task<List<HealthJourney>> GetAllWithDiaryByUserId(int id)
        {
            return await _context.HealthJourneys
                .Where(u => u.UserId == id)
                .Include(u => u.User)
                .Include(p => p.PsychologyDiaries)
                .Include(p => p.Treatment)
                .ToListAsync();
        }

        public async Task<List<HealthJourney>> GetAllWithPeriodicByUserId(int id)
        {
            return await _context.HealthJourneys
                .Where(u => u.UserId == id)
                .Include(u => u.User)
                .Include(p => p.PeriodicHealths)
                .Include(p => p.Treatment)
                .ToListAsync();
        }
    }
}
