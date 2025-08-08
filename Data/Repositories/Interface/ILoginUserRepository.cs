using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;

namespace GentleBlossom_BE.Data.Repositories.Interface
{
    public interface ILoginUserRepository : IGenericRepository<LoginUser>
    {
        Task<LoginUser?> GetUsnLoginAsync(string usn);
        Task<string?> GetPwLoginAsync(int loginId);
        Task<bool> CheckUsnExistAsync(string sđt);
        Task<LoginUser?> GetUsnLoginAdminAsync(string usn);
        Task<(bool Success, string? Email, string? ErrorMessage)> ForgotPasswordRequest(string username);
        Task<bool> ChangePassword(string password, int userId);
    }
}
