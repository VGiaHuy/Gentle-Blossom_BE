using GentleBlossom_BE.Data.DTOs;
using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileDTO userProfile)
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

            var updatedUserProfile = await _userProfileService.UpdateUserProfile(userProfile);

            if (updatedUserProfile == true)
            {

                return Ok(new API_Response<object>
                {
                    Success = true,
                    Message = "Cập nhật thông tin người dùng thành công!",
                    Data = null
                });
            }
            else
            {
                return BadRequest(new API_Response<object>
                {
                    Success = false,
                    Message = "Cập nhật thông tin người dùng thất bại!",
                    Data = null
                });
            }
        }
    }
}
