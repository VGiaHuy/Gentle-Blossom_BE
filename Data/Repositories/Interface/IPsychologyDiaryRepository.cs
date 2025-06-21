using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IPsychologyDiaryRepository : IGenericRepository<PsychologyDiary>
    {
        Task<List<PsychologyDiary>> GetAllByHealthJourneyId(int healthJourneyId);
    }
}
