using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IHealthJourneyRepository : IGenericRepository<HealthJourney>
    {
        Task<List<HealthJourney>> GetAllWithDiaryByUserId(int id);
        Task<List<HealthJourney>> GetAllWithPeriodicByUserId(int id);
        Task<List<HealthJourney>> GetAllByUserId(int id);
    }
}
