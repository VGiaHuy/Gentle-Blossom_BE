using GentleBlossom_BE.Data.DTOs.UserDTOs;
using GentleBlossom_BE.Data.Responses;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPost([FromQuery] int userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var data = await _postService.GetAllPost(userId, page, pageSize);

            return Ok(new API_Response<List<PostDTO>>
            {
                Success = true,
                Message = "Lấy danh sách bài viết thành công!",
                Data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPostsOfUserById([FromQuery] int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var data = await _postService.GetPostsOfUserById(id, page, pageSize);

            return Ok(new API_Response<List<PostDTO>>
            {
                Success = true,
                Message = "Lấy danh sách bài viết thành công!",
                Data = data
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDTO request)
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

            await _postService.CreatePostAsync(request);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Đăng tải thành công!",
                Data = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromForm] CreateCommentDTOs request)
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

            // Xử lý logic lưu bình luận
            await _postService.CreateCommentAsync(request);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Đăng tải thành công!",
                Data = null
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsByPostId([FromQuery] int postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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

            var data = await _postService.GetCommentsByPostIdAsync(postId, page, pageSize);

            return Ok(new API_Response<CommentPostResponseDTO>
            {
                Success = true,
                Message = "Lấy bình luận thành công!",
                Data = data
            });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLikePost([FromBody] ToggleLikePostDto request)
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

            int postId = request.PostId;
            int userId = request.UserId;

            var data = await _postService.ToggleLikePost(postId, userId);

            return Ok(new API_Response<object>
            {
                Success = true,
                Message = "Like bài viết thành công!",
                Data = null
            });
        }

        [HttpGet]
        public async Task<IActionResult> ProxyImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return BadRequest("URL is required");
            }

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");
                client.Timeout = TimeSpan.FromSeconds(10);

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, $"Failed to fetch image: {response.ReasonPhrase}");
                }

                var content = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "image/jpeg";
                return File(content, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching image: {ex.Message}");
            }
        }
    }
}
