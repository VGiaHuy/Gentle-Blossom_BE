using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        UserAuthService _userAuthService;

        public UserAuthController(UserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
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
            return Ok(new API_Response<UserProfileDTO>
            {
                Success = true,
                Message = "Đăng nhập thành công!",
                Data = userProfile
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new API_Response<UserProfileDTO>()
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
