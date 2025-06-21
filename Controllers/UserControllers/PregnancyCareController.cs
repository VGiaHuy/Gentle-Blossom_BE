using Azure;
using GentleBlossom_BE.Data.Constants;
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

        [HttpGet]
        public async Task<IActionResult> GetHealthJourney([FromQuery] int chatRoomId, [FromQuery] int expertId)
        {
            var response = await _pregnancyCareService.GetHealthJourney(chatRoomId, expertId);

            return Ok(new API_Response<List<HealthJourneyDTO>>
            {
                Success = true,
                Message = "Lấy dữ liệu thành công!",
                Data = response
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailHealthJourney([FromQuery] int trackingId, [FromQuery] int treatmentId)
        {
            if (treatmentId == Treatments.Periodic) 
            {
                var response = await _pregnancyCareService.GetAllPeriodicByHeathId(trackingId);

                return Ok(new API_Response<List<PeriodicHealthDTO>>
                {
                    Success = true,
                    Message = "Lấy dữ liệu thành công!",
                    Data = response
                });
            }
            else if (treatmentId == Treatments.Psychology)
            {
                var response = await _pregnancyCareService.GetAllPsychologyByHeathId(trackingId);

                return Ok(new API_Response<List<PsychologyDiaryDTO>>
                {
                    Success = true,
                    Message = "Lấy dữ liệu thành công!",
                    Data = response
                });
            }else
            {
                return BadRequest(new API_Response<object>
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ!",
                    Data = null
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMonitoringFormByHeathId([FromQuery] int heathId)
        {
            var response = await _pregnancyCareService.GetAllMonitoringFormByHeathId(heathId);

            return Ok(new API_Response<List<MonitoringFormDTO>>
            {
                Success = true,
                Message = "Lấy dữ liệu thành công!",
                Data = response
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateHealthJourney([FromBody] CreateHealthJourneyDTO createHealth)
        {
            if (createHealth == null)
            {
                return BadRequest(new API_Response<object>
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ!",
                    Data = null
                });
            }

            await _pregnancyCareService.CreateHealthJourney(createHealth);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Tạo hành trình sức khỏe thành công!",
                Data = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateMonitoring([FromBody] CreateMonitoringDTO createHealth)
        {
            if (createHealth == null)
            {
                return BadRequest(new API_Response<object>
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ!",
                    Data = null
                });
            }

            await _pregnancyCareService.CreateMonitoring(createHealth);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Tạo hành trình sức khỏe thành công!",
                Data = null
            });
        }
    }
}
