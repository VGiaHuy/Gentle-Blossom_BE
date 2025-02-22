using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories;

namespace GentleBlossom_BE.Services.UserServices
{
    public class UserAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAuthService(IUnitOfWork unitOfWork)  
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserProfile>> GetAllUserAsync()
        {
            return await _unitOfWork.UserProfile.GetAllAsync();
        }
    }
}
