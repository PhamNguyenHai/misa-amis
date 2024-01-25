using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IExcelRepository<TRequestDto, TEntity> : IExcelExportRepository
    {
        /// <summary>
        /// Hàm thực hiện dowload file mẫu để import ứng với nghiệp vụ
        /// </summary>
        /// <param name="filePath">đường dẫn của file cần lấy</param>
        /// <returns>Mangr byte của file mẫu</returns>
        /// Author: PNNHai
        /// Date
        Task<byte[]> DowloadTemplateFile(string filePath);

        /// <summary>
        /// Hàm thực hiện thêm nhiều
        /// </summary>
        /// <param name="entities">danh sách muốn thêm</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task InsertRangeAsync(List<TEntity> entities);

        /// <summary>
        /// Hàm thực hiện lấy dữ liệu từ cache bằng key
        /// </summary>
        /// <param name="key">key muốn lấy data từ cache</param>
        /// <returns>Dữ liệu được lưu trong cache thông qua key</returns>
        /// Author: PNNHai
        /// Date:
        object GetCache(string key);

        /// <summary>
        /// Hàm thực hiện cache dữ liệu value với key
        /// </summary>
        /// <param name="key">key để cache dữ liệu</param>
        /// <param name="value">dữ liệu muốn lưu vào cache</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        void SetCache(string key, object value);

        /// <summary>
        /// Hàm thực hiện cache dữ liệu value với key
        /// </summary>
        /// <param name="key">key muốn xóa cache dữ liệu</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        void RemoveByKeyCache(string key);

        /// <summary>
        /// Hàm thực hiện lấy danh sách dữ liệu đã cache vào để thực hiện thêm
        /// </summary>
        /// <param name="tableName">bảng muốn thêm</param>
        /// <returns>danh sách object</returns>
        List<object> GetListObjectByTableName(string tableName);
    }
}
