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
        /// Thực hiện lấy thông tin user thông qua thông tin đăng nhập người dùng 
        /// </summary>
        /// <param name="emailOrPhoneNumber">email hoặc số điện thoại đăng nhập</param>
        /// <param name="password">mật khẩu đăng nhập</param>
        /// <returns>Người dùng thỏa mãn</returns>
        /// Author: PNNHai
        /// Date:
        Task<User?> FindUserByLoginInforAsync(string emailOrPhoneNumber, string password);

        /// <summary>
        /// Thực hiện đổi mật khẩu cho tài khoản
        /// </summary>
        /// <param name="id">id người dùng cần đổi mật khẩu</param>
        /// <param name="newPassword">mật khẩu mới cần set</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task ChangePasswordAsync(Guid id, string newPassword);

        /// <summary>
        /// Thực hiện kiểm tra xem mật khẩu truyền vào có phải là mật khẩu hiện tại của người dùng với id không
        /// </summary>
        /// <param name="id">id người dùng cần kiểm tra</param>
        /// <param name="password">mật khẩu cần kiểm tra</param>
        /// <returns>true (nếu mật khẩu match) || false (nếu mật khẩu ko match)</returns>
        Task<bool> IsPasswordMatched(Guid id, string password);

        /// <summary>
        /// Thực hiện tìm người dùng thông qua email
        /// </summary>
        /// <param name="email">Email của người dùng muốn thực hiện tìm kiếm</param>
        /// <returns>Người dùng || null</returns>
        /// Author: PNNHai
        /// Date:
        Task<User?> FindUserByEmail(string email);

        /// <summary>
        /// Thực hiện tìm người dùng thông qua số điện thoại
        /// </summary>
        /// <param name="phoneNumber">SĐT của người dùng muốn thực hiện tìm kiếm</param>
        /// <returns>Người dùng || null</returns>
        /// Author: PNNHai
        /// Date:
        Task<User?> FindUserByPhoneNumber(string phoneNumber);      
    }
}