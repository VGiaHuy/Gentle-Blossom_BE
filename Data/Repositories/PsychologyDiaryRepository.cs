using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class PsychologyDiaryRepository : GenericRepository<PsychologyDiary>, IPsychologyDiaryRepository
    {
        public PsychologyDiaryRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<List<PsychologyDiary>> GetAllByHealthJourneyId(int healthJourneyId)
        {
            return await _context.PsychologyDiaries
                .Where(pd => pd.JourneyId == healthJourneyId)
                .Include(pd => pd.Journey)
                .Include(ph => ph.Journey.Treatment)
                .ToListAsync();
        }
    }
}
