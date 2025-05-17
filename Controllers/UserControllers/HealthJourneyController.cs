using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class HealthJourneyController : ControllerBase
    {
        private readonly HealthJourneyService _healthJourneyService;

        public HealthJourneyController(HealthJourneyService healthJourneyService) 
        {
            _healthJourneyService = healthJourneyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDiaryByUserId([FromQuery] int id)
        {
            var data = await _healthJourneyService.GetDiaryByUserId(id);

            return Ok(new API_Response<List<PsychologyDiaryDTO>>
            {
                Success = true,
                Message = "Lấy danh sách nhật ký thành công!",
                Data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPeriodicByUserId([FromQuery] int id)
        {
            var data = await _healthJourneyService.GetPeriodicByUserId(id);

            return Ok(new API_Response<List<PeriodicHealthDTO>>
            {
                Success = true,
                Message = "Lấy danh sách sức khỏe định kỳ thành công!",
                Data = data
            });
        }
    }
}
