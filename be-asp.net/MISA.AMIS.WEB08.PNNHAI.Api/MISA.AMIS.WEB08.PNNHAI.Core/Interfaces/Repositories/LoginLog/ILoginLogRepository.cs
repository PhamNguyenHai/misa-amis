using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface ILoginLogRepository
    {
        /// <summary>
        /// Thực hiện lấy thông tin nhật kí đăng nhập thông qua Mã định danh
        /// </summary>
        /// /// <param name="loginLogId">Mã định danh của nhật kí đăng nhập cần lấy</param>
        /// <returns></returns>
        Task<LoginLog?> FindLoginLogByIdAsync(Guid loginLogId);

        /// <summary>
        /// Thực hiện thay đổi thời gian logout của nhật kí họat động
        /// </summary>
        /// <param name="loginLogId">Mã định danh của nhật kí đăng nhập cần lấy</param>
        /// <param name="logoutDate">Thời gian đăng xuất cần thiết lập</param>
        /// <returns></returns>
        Task ChangeLogoutDateByLoginIdAsync(Guid loginLogId, DateTime logoutDate);

        /// <summary>
        /// Hàm thực hiện thêm nhật kí đăng nhập
        /// </summary>
        /// <param name="loginLogToInsert"></param>
        /// <returns></returns>
        Task InsertLoginLogAsync(LoginLog loginLogToInsert);
    }
}
