using GentleBlossom_BE.Data.DTOs;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileService _userProfileService;

        public UserProfileController(UserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProfile([FromQuery] int id)
        {
            var userProfile = await _userProfileService.GetAllUserProfile(id);

            return Ok(new API_Response<UserProfileViewModel>
            {
                Success = true,
                Message = "Lấy thông tin người dùng thành công!",
                Data = userProfile
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo([FromQuery] int id)
        {
            var userProfile = await _userProfileService.GetUserInfo(id);

            return Ok(new API_Response<UserProfileDTO>
            {
                Success = true,
                Message = "Lấy thông tin người dùng thành công!",
                Data = userProfile
            });
        }
    }
}
