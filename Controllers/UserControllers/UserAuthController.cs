using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using GentleBlossom_BE.Services.JWTService;
using Microsoft.AspNetCore.Authorization;
using GentleBlossom_BE.Services.EmailServices;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [AllowAnonymous]
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly UserAuthService _userAuthService;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;


        public UserAuthController(UserAuthService userAuthService, JwtService jwtService, EmailService emailService)
        {
            _userAuthService = userAuthService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(new API_Response<object>
                {
                    Success = false,
                    Message = string.Join(" ", errors),
                    Data = null
                });
            }

            var userProfile = await _userAuthService.Login(request);
            var JwtResponse = await _jwtService.CreateTokenUser(userProfile.FullName);

            var response = new LoginResponse
            {
                userProfileDTO = userProfile,
                AccessToken = JwtResponse.AccessToken,
                ExpiresIn = JwtResponse.ExpiresIn
            };
            return Ok(new API_Response<LoginResponse>
            {
                Success = true,
                Message = "Đăng nhập thành công!",
                Data = response
            });
        }

        [HttpPost]
        public async Task<IActionResult> CheckLoggedWithGoogle([FromBody] LoginWithGoogle request)
        {
            var userProfile = await _userAuthService.CheckLoggedWithGoogle(request);

            if (userProfile != null)
            {
                var JwtResponse = await _jwtService.CreateTokenUser(request.fullName);
                var response = new LoginResponse
                {
                    userProfileDTO = userProfile,
                    AccessToken = JwtResponse.AccessToken,
                    ExpiresIn = JwtResponse.ExpiresIn
                };
                return Ok(new API_Response<LoginResponse>
                {
                    Success = true,
                    Message = "Tài khoản hợp lệ!",
                    Data = response
                });
            }

            return Ok(new API_Response<LoginResponse>
            {
                Success = false,
                Message = "Tài khoản chưa đăng nhập bằng Google trước đó!",
                Data = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterForLoginGoogle([FromBody] CompleteProfile request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new API_Response<object>()
                {
                    Success = false,
                    Message = string.Join(" ", errors),
                    Data = null
                });
            }

            var register = await _userAuthService.RegisterForLoginGoogle(request);
            var JwtResponse = await _jwtService.CreateTokenUser(request.FullName);

            var response = new LoginResponse
            {
                userProfileDTO = register,
                AccessToken = JwtResponse.AccessToken,
                ExpiresIn = JwtResponse.ExpiresIn
            };
            return Ok(new API_Response<LoginResponse>
            {
                Success = true,
                Message = "Đăng nhập thành công!",
                Data = response
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new API_Response<object>()
                {
                    Success = false,
                    Message = string.Join(" ", errors),
                    Data = null
                });
            }

            var register = await _userAuthService.Register(request);

            return Ok(new API_Response<object>()
            {
                Success = true,
                Message = "Đăng ký thành công!",
                Data = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var (otpToken, userEmail) = await _userAuthService.SendOtpToEmail(request.Username);

            var maskedEmail = MaskEmail(userEmail);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = $"Email xác thực đã được gửi tới {maskedEmail}",
                Data = new { OtpToken = otpToken, Email = userEmail }
            });
        }

        [HttpPost]
        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var isValid = _jwtService.ValidateOtpToken(request.OtpToken, request.Email, request.Otp);
            if (!isValid)
                return BadRequest(new API_Response<object> { Success = false, Message = "OTP không hợp lệ hoặc đã hết hạn.", Data = null });

            return Ok(new API_Response<object> { Success = true, Message = "OTP hợp lệ.", Data = null });
        }

        private string MaskEmail(string email)
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

        //[HttpPost]
        //public async Task<IActionResult> insertPassword()
        //{
        //    await _userAuthService.InsertPassword();

        //    return Ok();
        //}
    }
}
