using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using GentleBlossom_BE.Services.JWTService;
using Microsoft.AspNetCore.Authorization;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [AllowAnonymous]
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly UserAuthService _userAuthService;
        private readonly JwtService _jwtService;

        public UserAuthController(UserAuthService userAuthService, JwtService jwtService)
        {
            _userAuthService = userAuthService;
            _jwtService = jwtService;
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

        //[HttpPost]
        //public async Task<IActionResult> insertPassword()
        //{
        //    await _userAuthService.InsertPassword();

        //    return Ok();
        //}
    }
}
