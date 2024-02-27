using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.WEB08.PNNHAI.Core;
using System;

namespace MISA.AMIS.WEB08.PNNHAI.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : BaseController<UserDto, UserCreateDto, UserUpdateDto>
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            var result = await _userService.LoginAsync(userLogin);
            return Ok(result);
        }

        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _userService.RefreshTokenAsync();
            return Ok(new { accessToken = result } );
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Loutout()
        {
            await _userService.LogoutAsync();
            return Ok("Đăng xuất thành công");
        }

        public async override Task<IActionResult> FilterPaging(FilterInput filterInput)
        {
            return Ok("Tính năng chưa được khai thác");
        }
    }
}
