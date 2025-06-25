using Gentle_Blossom_FE.Areas.Admin.Data;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.AdminServices;
using GentleBlossom_BE.Services.JWTService;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.AdminControllers
{
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly AccountService _accountService;

        public AccountController(JwtService jwtService, AccountService accountService)
        {
            _jwtService = jwtService;
            _accountService = accountService;
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

            var adminProfile = await _accountService.Login(request);
            var JwtResponse = await _jwtService.CreateTokenUser(adminProfile.FullName);

            var response = new LoginAdminResponse
            {
                adminProfileDTO = adminProfile,
                AccessToken = JwtResponse.AccessToken,
                ExpiresIn = JwtResponse.ExpiresIn
            };
            return Ok(new API_Response<LoginAdminResponse>
            {
                Success = true,
                Message = "Đăng nhập thành công!",
                Data = response
            });
        }
    }
}
