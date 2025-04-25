using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        PostService _postService;

        public PostController(PostService postService) 
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPost()
        {
            var data = await _postService.GetAllPost();

            return Ok(new API_Response<List<PostDTO>>
            {
                Success = true,
                Message = "Lấy danh sách bài viết thành công!",
                Data = data
            });
        }
    }
}
