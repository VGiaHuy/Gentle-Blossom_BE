using DocumentFormat.OpenXml.Spreadsheet;
using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Data.Repositories
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(GentleBlossomContext context) : base(context)
        {
        }

        public async Task<UserProfile?> GetByIdWithUserTypeAndExpertAsync(int id)
        {
            return await _context.UserProfiles
                .Include(u => u.UserType)
                .Include(u => u.Expert)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var data = await _context.UserProfiles.FirstOrDefaultAsync(a => a.Email == email);

            return data != null;
        }

        public async Task<bool> CheckPhoneNumbExistAsync(string phoneNumb)
        {
            var data = await _context.UserProfiles.FirstOrDefaultAsync(a => a.PhoneNumber == phoneNumb);

            return data != null;
        }

        public async Task<UserProfile> GetAdminProfile(int userId)
        {
            return await _context.UserProfiles
                .Where(u => u.UserId == userId)
                .Include(u => u.Administrator)
                    .ThenInclude(a => a.Role)
                .FirstAsync();
        }

        public async Task<UserProfile?> CheckLoginGgExistAsync(string email)
        {
            return await _context.UserProfiles
                .Include(u => u.LoginUser)
                .FirstOrDefaultAsync(u => u.Email == email && u.LoginUser.TypeLogin == LoginType.Google);
        }
    }
}
