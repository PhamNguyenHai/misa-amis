using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IUserService : IBaseService<UserDto, UserCreateDto, UserUpdateDto>
    {
        /// <summary>
        /// Thực hiện đăng nhập tài khoản
        /// </summary>
        /// <param name="userToLogin">Thông tin đăng nhập người dùng</param>
        /// <returns>Thông tin đăng nhập thành công (access token, user name, user id)</returns>
        /// Author: PNNHai
        /// Date:
        Task<LoginedUserInfor> LoginAsync(UserLoginDto userToLogin);

        /// <summary>
        /// Thực hiện đăng xuất
        /// </summary>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task LogoutAsync();

        /// <summary>
        /// Thực hiện duy trì đăng nhập refresh token
        /// </summary>
        /// <returns>AccessToken mới</returns>
        /// Author: PNNHai
        /// Date:
        Task<string> RefreshTokenAsync();

        /// <summary>
        /// Thực hiện đổi mật khẩu cho tài khoản
        /// </summary>
        /// <param name="userPasswordChange">Thông tin tài khoản cần thực hiện thay đổi và mật khẩu thay đổi</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task ChangePasswordAsync(UserPasswordChangeDto userPasswordChange);

        /// <summary>
        /// Thực hiện đăng kí tài khoản người dùng
        /// </summary>
        /// <param name="registerInfor">Thông tin đăng kí tài khoản</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        //Task RegisterUserAccountAsync(UserRegisterDto registerInfor);

        /// <summary>
        /// Thực hiện đăng kí tài khoản quản trị
        /// </summary>
        /// <param name="registerInfor">Thông tin đăng kí tài khoản</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        //Task RegisterAdministratorAccountAsync(UserRegisterDto registerInfor);
    }
}
