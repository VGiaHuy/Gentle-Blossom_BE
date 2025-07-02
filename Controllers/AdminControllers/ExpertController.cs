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

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { Message = "File không hợp lệ hoặc bị trống." });
            }

            // Kiểm tra định dạng tệp
            var allowedExtensions = new[] { ".xlsx", ".xls" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { Message = "Chỉ chấp nhận file Excel với định dạng .xlsx hoặc .xls." });
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    // Gọi service để xử lý file
                    var successfulRecords = _expertService.ProcessExcelFile(stream);

                    // Lưu các bản ghi hợp lệ và lỗi vào bảng tạm
                     await _expertService.SaveToTempTable(successfulRecords);

                    if (successfulRecords.Count == 0)
                    {
                        return Ok(new API_Response<object>
                        {
                            Success = false,
                            Message = "Toàn bộ file chứa lỗi, vui lòng kiểm tra và sửa lại.",
                            Data = null,
                        });
                    }

                    return Ok(new API_Response<object>
                    {
                        Success = true,
                        Message = "File đã được xử lý thành công. Một số dòng chứa lỗi cần được chỉnh sửa.",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Có lỗi xảy ra trong quá trình xử lý file.",
                    Details = ex.Message
                });
            }
        }
    }
}
