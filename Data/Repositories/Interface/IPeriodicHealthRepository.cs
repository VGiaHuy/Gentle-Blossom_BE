using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IPeriodicHealthRepository : IGenericRepository<PeriodicHealth>
    {
        Task<List<PeriodicHealth>> GetAllByHealthJourneyId(int healthJourneyId);
    }
}
