using AutoMapper;
using GentleBlossom_BE.Data.Constants;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Exceptions;
using GentleBlossom_BE.Services.EmailServices;
using GentleBlossom_BE.Services.JWTService;
using System.IdentityModel.Tokens.Jwt;

namespace GentleBlossom_BE.Services.UserServices
{
    public class UserAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly GentleBlossomContext _context;

        private readonly JwtService _jwtService;

        private readonly EmailService _emailService;

        public UserAuthService(IUnitOfWork unitOfWork, IMapper mapper, GentleBlossomContext context, JwtService jwtService, EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        public async Task<UserProfileDTO> Login(LoginRequestDTO request)
        {
            var user = await _unitOfWork.LoginUser.GetUsnLoginAsync(request.Username);

            if (user == null)
                throw new UnauthorizedException("Tài khoản không tồn tại!");

            var hashedPassword = await _unitOfWork.LoginUser.GetPwLoginAsync(user.LoginId);
            if (!BCrypt.Net.BCrypt.Verify(request.Password, hashedPassword))
                throw new UnauthorizedException("Mật khẩu không chính xác!");

            var userInfo = await _unitOfWork.UserProfile.GetByIdAsync(user.UserId);
            return _mapper.Map<UserProfileDTO>(userInfo);
        }

        public async Task<UserProfileDTO> CheckLoggedWithGoogle(LoginWithGoogle request)
        {
            try
            {
                var checkLoginGg = await _unitOfWork.UserProfile.CheckLoginGgExistAsync(request.email);

                if (checkLoginGg != null)
                {
                    return _mapper.Map<UserProfileDTO>(checkLoginGg);
                }

                bool checkExistEmail = await _unitOfWork.UserProfile.CheckEmailExistAsync(request.email);
                if (checkExistEmail)
                {
                    throw new BadRequestException("Email đã được sử dụng đăng ký một tài khoản khác!");
                }

                throw new NotFoundException("Không tìm thấy thông tin!");
            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

        public async Task<UserProfileDTO> RegisterForLoginGoogle(CompleteProfile data)
        {
            try
            {
                bool checkExistPhoneNumb = await _unitOfWork.UserProfile.CheckPhoneNumbExistAsync(data.PhoneNumber);
                if (checkExistPhoneNumb)
                {
                    throw new BadRequestException("Số điện thoại đã được sử dụng!");
                }

                var login = new LoginUser
                {
                    UserName = data.GoogleId,
                    Password = data.GoogleId,
                    TypeLogin = LoginType.Google,
                };

                var userInfo = new UserProfile
                {
                    FullName = data.FullName,
                    BirthDate = data.DateOfBirth,
                    Email = data.Email,
                    Gender = false,
                    PhoneNumber = data.PhoneNumber,
                    UserTypeId = UserTypeName.User,
                };

                // Save data
                await _unitOfWork.UserProfile.AddAsync(userInfo);
                var result_profile = await _unitOfWork.SaveChangesAsync(useTransaction: false);

                login.UserId = userInfo.UserId;
                login.Password = BCrypt.Net.BCrypt.HashPassword(login.Password);
                await _unitOfWork.LoginUser.AddAsync(login);
                var result_login = await _unitOfWork.SaveChangesAsync(useTransaction: false);

                return new UserProfileDTO { UserId = userInfo.UserId };
            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
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

            bool checkExitsUsn = await _unitOfWork.LoginUser.CheckUsnExistAsync(register.UserName);
            if (checkExitsUsn)
            {
                throw new BadRequestException("Tên đăng nhập đã được sử dụng!");
            }

            try
            {
                var userInfo = _mapper.Map<UserProfile>(register);
                var login = _mapper.Map<LoginUser>(register);

                // Save data
                userInfo.UserTypeId = 3;
                await _unitOfWork.UserProfile.AddAsync(userInfo);
                var result_profile = await _unitOfWork.SaveChangesAsync(useTransaction: false);

                login.UserId = userInfo.UserId;
                login.Password = BCrypt.Net.BCrypt.HashPassword(login.Password);
                await _unitOfWork.LoginUser.AddAsync(login);
                var result_login = await _unitOfWork.SaveChangesAsync(useTransaction: false);

                return true;
            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

        public async Task<string> ForgotPasswordRequest(string username)
        {
            try
            {
                var email = await _unitOfWork.LoginUser.ForgotPasswordRequest(username);
                if (!email.Success)
                {
                    throw new BadRequestException(email.ErrorMessage!);
                }

                return email.Email!;
            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

        public async Task<(string, string)> SendOtpToEmail(string username)
        {
            var email = await ForgotPasswordRequest(username);
            var otpToken = await _jwtService.CreateOtpToken(email);

            // Giải mã OTP từ token để gửi email
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(otpToken);
            var otp = jwt.Claims.First(c => c.Type == "otp").Value;

            string templatePath = Path.Combine(AppContext.BaseDirectory, "Content", "OtpTemplate.html");
            string htmlBody = await File.ReadAllTextAsync(templatePath);

            htmlBody = htmlBody.Replace("{EMAIL}", email).Replace("{OTP}", otp);

            // Gửi email
            await _emailService.SendEmailAsync(email, "OTP Verification", htmlBody);

            return (otpToken, email);
        }

        public string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                return email;

            var parts = email.Split('@');
            var name = parts[0];
            var domain = parts[1];

            if (name.Length <= 2)
                return $"{name.Substring(0, 1)}*@{domain}";

            return $"{name.Substring(0, 2)}{new string('*', name.Length - 2)}@{domain}";
        }

        public async Task<bool> ChangePassword(string password, string email)
        {
            int userId = await _unitOfWork.UserProfile.GetUserIdByEmail(email);
            if (userId == 0)
            {
                throw new BadRequestException("Không tìm thấy thông tin tài khoản");
            }

            var updatePassword = await _unitOfWork.LoginUser.ChangePassword(password, userId);
            if (!updatePassword)
            {
                throw new InternalServerException("Không thể cập nhật mật khẩu");
            }

            return true;
        }
    }
}
