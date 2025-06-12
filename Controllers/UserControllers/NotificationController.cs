using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notification;

        public NotificationController(NotificationService notification)
        {
            _notification = notification;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotification([FromQuery] int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var data = await _notification.GetNotification(userId, page, pageSize);

            return Ok(new API_Response<NotificationResponseDTO>
            {
                Success = true,
                Message = "Lấy danh sách thông báo thành công!",
                Data = data
            });
        }

        [HttpPost]
        public async Task<IActionResult> ReadNotice([FromBody] string notificationId)
        {
            var data = await _notification.ReadNotice(notificationId);

            return Ok(new API_Response<NotificationResponseDTO>
            {
                Success = true,
                Message = "Lấy danh sách thông báo thành công!",
                Data = null
            });
        }
    }
}
