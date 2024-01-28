using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IBaseStatisticalService
    {
        /// <summary>
        /// Hàm thực hiện thống kê dữ liệu theo các cụm thông qua field key
        /// </summary>
        /// <param name="propertyKey">thuộc tính có trong bảng cần thống kê theo</param>
        /// <returns>Danh sách dữ liệu thống kê</returns>
        Task<IEnumerable<StatisticalDto>> StatisticalByPropertyKey(string propertyKey);
    }
}
