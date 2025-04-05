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

        public async Task<bool> checkEmailExistAsync(string email)
        {
            var data = await _context.UserProfiles.FirstOrDefaultAsync(a => a.Email == email);

            return data != null;
        }

        public async Task<bool> checkPhoneNumbExistAsync(string phoneNumb)
        {
            var data = await _context.UserProfiles.FirstOrDefaultAsync(a => a.PhoneNumber == phoneNumb);

            return data != null;
        }
    }
}
