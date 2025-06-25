using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface IUserProfileRepository : IGenericRepository<UserProfile>
    {
        Task<UserProfile?> GetByIdWithUserTypeAndExpertAsync(int id);

        Task<bool> CheckEmailExistAsync(string email);
        Task<bool> CheckPhoneNumbExistAsync(string sđt);
        Task<UserProfile> GetAdminProfile(int userId);

    }
}
