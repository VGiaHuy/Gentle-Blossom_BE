using GentleBlossom_BE.Data.DTOs.AminDTOs;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.AdminServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.AdminControllers
{
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
    public class ExpertController : ControllerBase
    {
        private readonly ExpertService _expertService;

        public ExpertController(ExpertService expertService)
        {
            _expertService = expertService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExperts(int page, int pageSize)
        {
            var data = await _expertService.GetAllExpert(page, pageSize);

            return Ok(new API_Response<ExpertResponse>
            {
                Success = true,
                Message = "Lấy dữ liệu thành công!",
                Data = data
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExpert([FromQuery] int expertId)
        {
            await _expertService.DeleteExpert(expertId);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Xóa Chuyên gia thành công!",
                Data = null
            });
        }
    }
}
