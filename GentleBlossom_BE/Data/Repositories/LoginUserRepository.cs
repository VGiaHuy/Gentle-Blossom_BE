using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GentleBlossom_BE.Data.Repositories
{
    public class LoginUserRepository : GenericRepository<LoginUser>, ILoginUserRepository
    {
        public LoginUserRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<LoginUser?> getUsnLoginAsync(string userName)
        {
            return await _context.LoginUsers
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<string?> getPwLoginAsync(int loginId)
        {
            return (await _context.LoginUsers.FirstOrDefaultAsync(a => a.LoginId == loginId))?.Password;
        }

        public async Task<bool> checkUsnExistAsync(string userName)
        {
            var data = await _context.LoginUsers.FirstOrDefaultAsync(a => a.UserName == userName);

            return data != null;
        }
    }
}
