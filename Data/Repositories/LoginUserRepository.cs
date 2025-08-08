using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GentleBlossom_BE.Data.Repositories
{
    public class LoginUserRepository : GenericRepository<LoginUser>, ILoginUserRepository
    {
        public LoginUserRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<LoginUser?> GetUsnLoginAsync(string userName)
        {
            return await _context.LoginUsers
                .Where(u => u.User.UserTypeId != 1)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<string?> GetPwLoginAsync(int loginId)
        {
            return (await _context.LoginUsers.FirstOrDefaultAsync(a => a.LoginId == loginId))?.Password;
        }

        public async Task<bool> CheckUsnExistAsync(string userName)
        {
            var data = await _context.LoginUsers.FirstOrDefaultAsync(a => a.UserName == userName);

            return data != null;
        }

        public async Task<LoginUser?> GetUsnLoginAdminAsync(string usn)
        {
            return await _context.LoginUsers
                .Include(u => u.User)
                .Where(u => u.User.UserTypeId == 1)
                .FirstOrDefaultAsync(u => u.UserName == usn);
        }

        public async Task<(bool Success, string? Email, string? ErrorMessage)> ForgotPasswordRequest(string username)
        {
            var email = await _context.LoginUsers
                        .Where(u => u.UserName == username)
                        .Select(u => u.User != null ? u.User.Email : null)
                        .FirstOrDefaultAsync();

            if (email == null)
            {
                return (false, null, "Người dùng không tồn tại");
            }

            return (true, email, null);
        }

        public async Task<bool> ChangePassword(string password, int userId)
        {
            var user = await _context.LoginUsers.Where(u => u.UserId == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new BadRequestException("Thông tin tài khoản không tồn tại!");
            }

            user.Password = password;
            _context.SaveChanges();

            return true;
        }
    }
}
