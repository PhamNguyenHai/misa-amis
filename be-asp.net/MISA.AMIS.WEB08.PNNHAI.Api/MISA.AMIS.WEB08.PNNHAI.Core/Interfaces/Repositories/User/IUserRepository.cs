using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IUserRepository : IBaseRepository<User, UserModel>
    {

        /// <summary>
        /// Thực hiện kiểm tra thông tin đăng nhập có chính xác không
        /// </summary>
        /// <param name="emailOrPhoneNumber">email hoặc số điện thoại đăng nhập</param>
        /// <param name="password">mật khẩu đăng nhập</param>
        /// <returns>Người dùng thỏa mãn</returns>
        /// Author: PNNHai
        /// Date:
        Task<User?> CheckLoginInforAsync(string emailOrPhoneNumber, string password);

        /// <summary>
        /// Thực hiện thu hồi token theo id
        /// </summary>
        /// <param name="tokenId">mã định danh token</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task RevokeTokenAsync(Guid tokenId);


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
        /// <param name="role">Quyền cần đăng kí</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        //Task RegisterAccountAsync(UserRegisterDto registerInfor, UserRole role);

        /// <summary>
        /// Thực hiện tìm user theo id
        /// </summary>
        /// <param name="userId">Mã định danh người dùng</param>
        /// <returns>Người dùng</returns>
        //Task<User?> FindByIdAsync(Guid userId);
    }
}