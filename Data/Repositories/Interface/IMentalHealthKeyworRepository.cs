using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IMentalHealthKeyworRepository : IGenericRepository<MentalHealthKeyword>
    {
        Task<(List<MentalHealthKeyword>, int)> GetAllKeyWordAsync(int page, int pageSize);
        Task<bool> GetByKeywordAsync(string keyword);
    }
}
