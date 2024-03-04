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
        /// Thực hiện đổi mật khẩu cho tài khoản
        /// </summary>
        /// <param name="userPasswordChange">Thông tin tài khoản cần thực hiện thay đổi và mật khẩu thay đổi</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task ChangePasswordAsync(UserPasswordChangeDto userPasswordChange);

        /// <summary>
        /// Thực hiện check trùng email người dùng
        /// </summary>
        /// <param name="email">Email cần check</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task CheckUserExistByEmail(string email);

        /// <summary>
        /// Thực hiện check trùng sđt người dùng
        /// </summary>
        /// <param name="phoneNumber">SDT cần check</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task CheckUserExistByPhoneNumber(string phoneNumber);

        /// <summary>
        /// Thực hiện check xem email muốn update có thỏa mãn không
        /// </summary>
        /// <param name="id">Id người dùng cần update</param>
        /// <param name="email">Email muốn update</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task CheckUserEmailUpdateToExistedEmail(Guid id, string email);

        /// <summary>
        /// Thực hiện check xem sđt muốn update có thỏa mãn không
        /// </summary>
        /// <param name="id">Id người dùng cần update</param>
        /// <param name="phoneNumber">sđt muốn update</param>
        /// <returns></returns>
        Task CheckUserPhoneNumberUpdateToExistedPhoneNumber(Guid id, string phoneNumber);
    }
}