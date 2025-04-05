using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface ILoginUserRepository : IGenericRepository<LoginUser>
    {
        Task<LoginUser?> getUsnLoginAsync(string usn);
        Task<string?> getPwLoginAsync(int loginId);
        Task<bool> checkUsnExistAsync(string sđt);

    }
}
