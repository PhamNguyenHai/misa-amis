using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.WEB08.PNNHAI.Core;
using System;

namespace MISA.AMIS.WEB08.PNNHAI.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseController<UserDto, UserCreateDto, UserUpdateDto>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            var result = await _userService.LoginAsync(userLogin);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _userService.RefreshTokenAsync();
            return Ok(new { accessToken = result } );
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Loutout()
        {
            await _userService.LogoutAsync();
            return Ok("Đăng xuất thành công");
        }

        [HttpPut("Change-Password/{userId}")]
        public async Task<IActionResult> ChangPassword(Guid userId, [FromBody] UserPasswordChangeDto userPasswordChange)
        {
            await _userService.ChangePasswordAsync(userId, userPasswordChange);
            return Ok("Đổi mật khẩu thành công");
        }

        [HttpPut("Reset-Password/{userId}")]
        public async Task<IActionResult> ResetPassword(Guid userId)
        {
            await _userService.ResetPassword(userId);
            return Ok("Reset mật khẩu thành công");
        }

        [HttpGet("Login-Log/{userId}")]
        public async Task<IActionResult> GetUserLoginLogs(Guid userId)
        {
            var loginLogs = await _userService.GetUserLoginLogsById(userId);
            return Ok(loginLogs);
        }
    }
}
