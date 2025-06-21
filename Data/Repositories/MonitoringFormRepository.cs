using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class MonitoringFormRepository : GenericRepository<MonitoringForm>, IMonitoringFormRepository
    {
        public MonitoringFormRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<List<MonitoringForm>> GetAllMonitoringFormByHeathId(int healthJourneyId)
        {
            return await _context.MonitoringForms
                .Where(mf => mf.JourneyId == healthJourneyId)
                .Include(mf => mf.Expert.User)
                .ToListAsync();
        }
    }
}
