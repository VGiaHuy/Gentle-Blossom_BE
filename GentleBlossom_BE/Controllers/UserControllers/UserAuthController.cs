using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GentleBlossom_BE.Controllers.UserControllers
{
    [Route("api/user/[controller]/[action]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        UserAuthService _userAuthService;

        public UserAuthController(UserAuthService userAuthService) 
        {
            _userAuthService = userAuthService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userAuthService.GetAllUserAsync();
            return Ok(users);
        }
    }
}
