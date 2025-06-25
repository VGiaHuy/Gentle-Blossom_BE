using GentleBlossom_BE.Data.DTOs.AminDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.AdminServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.AdminControllers
{
    [Route("api/admin/[controller]/[action]")]
    [ApiController]
    public class MentalHealthKeywordsController : ControllerBase
    {
        private readonly MentalHealthKeywordService _keywordService;

        public MentalHealthKeywordsController(MentalHealthKeywordService keywordService) 
        {
            _keywordService = keywordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMentalHealthKeyword(int page, int pageSize)
        {
            var data = await _keywordService.GetAllMentalHealthKeyword(page, pageSize);

            return Ok(new API_Response<MentalHealthKeywordResponse>
            {
                Success = true,
                Message = "Lấy dữ liệu thành công!",
                Data = data
            });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMentalHealthKeyword([FromQuery] int keywordId)
        {
            await _keywordService.DeleteMentalHealthKeyword(keywordId);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Xóa từ khóa thành công!",
                Data = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMentalHealthKeyword([FromBody] MentalHealthKeyword keyword)
        {
            await _keywordService.UpdateMentalHealthKeyword(keyword);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Cập nhật từ khóa thành công!",
                Data = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddMentalHealthKeyword([FromBody] MentalHealthKeyword keyword)
        {
            await _keywordService.AddMentalHealthKeyword(keyword);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Thêm từ khóa thành công!",
                Data = null
            });
        }
    }
}
