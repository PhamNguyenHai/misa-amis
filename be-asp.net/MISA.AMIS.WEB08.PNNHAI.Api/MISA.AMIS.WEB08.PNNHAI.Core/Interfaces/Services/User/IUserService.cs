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
        /// <param name="id">Mã định danh của người dùng cần thay mật khẩu</param>
        /// <param name="userPasswordChange">Thông tin tài khoản cần thực hiện thay đổi và mật khẩu thay đổi</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task ChangePasswordAsync(Guid id, UserPasswordChangeDto userPasswordChange);

        /// <summary>
        /// Thực hiện reset mật khẩu người dùng
        /// </summary>
        /// <param name="id">Id tài khoản cần reset</param>
        /// <returns></returns>
        Task ResetPassword(Guid id);

        /// <summary>
        /// Thực hiện lấy nhật kí đăng nhập, đăng xuất thông qua của người dùng id
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <returns></returns>
        Task<IEnumerable<LoginLogDto>> GetUserLoginLogsById(Guid userId);
    }
}
