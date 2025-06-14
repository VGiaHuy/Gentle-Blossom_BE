using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class PregnancyCareController : ControllerBase
    {
        private readonly PregnancyCareService _pregnancyCareService;

        public PregnancyCareController(PregnancyCareService pregnancyCareService)
        {
            _pregnancyCareService = pregnancyCareService;
        }

        [HttpPost]
        public async Task<IActionResult> ConnectMessage([FromBody] ConnectMessageDTO connectMessage)
        {
            var response = await _pregnancyCareService.ConnectMessageAsync(connectMessage);

            if (response)
            {
                return Ok(new API_Response<object>
                {
                    Success = true,
                    Message = "Kết nối thành công!",
                    Data = null
                });
            }
            else
            {
                return BadRequest(new API_Response<object>
                {
                    Success = false,
                    Message = "Không thể kết nối!",
                    Data = null
                });
            }
        }
    }
}
