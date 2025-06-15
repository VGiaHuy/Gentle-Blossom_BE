using AutoMapper;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GentleBlossom_BE.Services.UserServices
{
    public class UserAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserAuthService(IUnitOfWork unitOfWork, IMapper mapper, GentleBlossomContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileDTO> Login(LoginRequestDTO request)
        {
            var user = await _unitOfWork.LoginUser.GetUsnLoginAsync(request.Username);

            if (user == null)
                throw new UnauthorizedException("Tài khoản không tồn tại!");

            var hashedPassword = await _unitOfWork.LoginUser.GetPwLoginAsync(user.LoginId);
            if (!BCrypt.Net.BCrypt.Verify(request.Password, hashedPassword))
                throw new UnauthorizedException("Mật khẩu không chính xác!");

            var userInfo = await _unitOfWork.UserProfile.GetByIdAsync(user.LoginId);
            return _mapper.Map<UserProfileDTO>(userInfo);
        }

        public async Task<bool> Register(RegisterViewModel register)
        {
            // Check data
            bool checkExistEmail = await _unitOfWork.UserProfile.CheckEmailExistAsync(register.Email);
            if (checkExistEmail)
            {
                throw new BadRequestException("Email đã được sử dụng!");
            }

            bool checkExistPhoneNumb = await _unitOfWork.UserProfile.CheckPhoneNumbExistAsync(register.PhoneNumber);
            if (checkExistPhoneNumb)
            {
                throw new BadRequestException("Số điện thoại đã được sử dụng!");
            }

            bool checkExitsUsn = await _unitOfWork.LoginUser.CheckUsnExistAsync(register.Username);
            if (checkExitsUsn)
            {
                throw new BadRequestException("Tên đăng nhập đã được sử dụng!");
            }

            // Mapping
            var userInfo = _mapper.Map<UserProfile>(register);
            var login = _mapper.Map<LoginUser>(register);

            // Save data
            userInfo.UserTypeId = 3;
            await _unitOfWork.UserProfile.AddAsync(userInfo);
            var result_profile = await _unitOfWork.SaveChangesAsync(useTransaction: false);

            login.UserId = userInfo.UserId;
            await _unitOfWork.LoginUser.AddAsync(login);
            var result_login = await _unitOfWork.SaveChangesAsync(useTransaction: false);

            return true;
        }


        //public async Task<API_Response<RegisterRequestDTO>> register(RegisterRequestDTO request)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        return new API_Response<RegisterRequestDTO>()
        //        {
        //            Success = false,
        //            Message= ex.Message,
        //            Data = null
        //        };
        //    }
        //}

        //public async Task InsertPassword()
        //{
        //    var phoneNumbers = new List<string>
        //    {
        //        "0901234567", "0934455778", "0923456789", "0934567890", "0945678901",
        //        "0956789012", "0967890123", "0978901234", "0912233556", "0990123456",
        //        "0990123451", "0901122334", "0912233445", "0923344556", "0934455667",
        //        "0945566778", "0956677889", "0967788990", "0978899001", "0989900112",
        //        "0990011223", "0978899112", "0989900223", "0990011334"
        //    };

        //    foreach (var phone in phoneNumbers)
        //    {
        //        var userId = await _context.UserProfiles
        //                                   .Where(up => up.PhoneNumber == phone)
        //                                   .Select(up => up.UserId)
        //                                   .FirstOrDefaultAsync();

        //        if (userId != 0)
        //        {
        //            string bcryptPassword = BCrypt.Net.BCrypt.HashPassword(phone);

        //            var loginUser = new LoginUser
        //            {
        //                UserName = phone,
        //                Password = bcryptPassword,
        //                UserId = userId
        //            };

        //            _context.LoginUsers.Add(loginUser);
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //}


    }
}
