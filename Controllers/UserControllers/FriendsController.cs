using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly FriendsService _friendsService;

        public FriendsController(FriendsService friendsService) 
        {
            _friendsService = friendsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExperts()
        {
            var response = await _friendsService.GetAllExperts();

            return Ok(new API_Response<List<ExpertProfileDTO>>
            {
                Success = true,
                Message = "Lấy dữ liệu thành công!",
                Data = response
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetExpertById([FromQuery] int expertId)
        {
            var response = await _friendsService.GetExpertById(expertId);

            return Ok(new API_Response<ExpertProfileDTO>
            {
                Success = true,
                Message = "Lấy dữ liệu thành công!",
                Data = response
            });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] RequestConnectChat connectChat)
        {
            var response = await _friendsService.SendMessage(connectChat);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Kết nối thành công!",
                Data = response
            });
        }
    }
}
