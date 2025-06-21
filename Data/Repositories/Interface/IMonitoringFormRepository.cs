using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IMonitoringFormRepository : IGenericRepository<MonitoringForm>
    {
        Task<List<MonitoringForm>> GetAllMonitoringFormByHeathId(int healthJourneyId);
    }
}
