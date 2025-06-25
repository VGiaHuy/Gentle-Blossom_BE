using AutoMapper;
using GentleBlossom_BE.Data.DTOs.AminDTOs;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;

namespace GentleBlossom_BE.Services.AdminServices
{
    public class AccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AdminProfileDTO> Login(LoginRequestDTO request)
        {
            var user = await _unitOfWork.LoginUser.GetUsnLoginAdminAsync(request.Username);

            if (user == null)
                throw new UnauthorizedException("Tài khoản không tồn tại!");

            var hashedPassword = await _unitOfWork.LoginUser.GetPwLoginAsync(user.LoginId);
            if (!BCrypt.Net.BCrypt.Verify(request.Password, hashedPassword))
                throw new UnauthorizedException("Mật khẩu không chính xác!");

            try
            {
                var adminProfile = await _unitOfWork.UserProfile.GetAdminProfile(user.UserId);
                var data = _mapper.Map<AdminProfileDTO>(adminProfile);
                return data;
            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }
    }
}
