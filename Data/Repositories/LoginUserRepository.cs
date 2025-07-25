﻿using GentleBlossom_BE.Data.Models;
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
    }
}
