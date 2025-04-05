using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IUserProfileRepository : IGenericRepository<UserProfile>
    {
        Task<bool> checkEmailExistAsync(string email);
        Task<bool> checkPhoneNumbExistAsync(string sđt);
    }
}
