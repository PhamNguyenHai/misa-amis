using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IExcelService<TRespondDto> : IExcelExportService
    {
        /// <summary>
        /// Hàm thực hiện dowload file mẫu để import ứng với nghiệp vụ
        /// </summary>
        /// <param name="workingObjectTable">bảng muốn thực hiện lấy file</param>
        /// <returns>Mangr byte của file mẫu</returns>
        /// Author: PNNHai
        /// Date
        Task<byte[]> DowloadTemplateFile(string workingObjectTable);

        /// <summary>
        /// Thực hiện đọc file excel được truyền lên
        /// </summary>
        /// <param name="importFile">file truyền lên</param>
        /// <param name="sheetUsed">sheet sử dụng</param>
        /// <param name="workingObjectTable">table sẽ thực hiện import vào</param>
        /// <returns>Danh sách dữ liệu các dòng sau khi đọc kèm theo trạng thái lỗi (nếu có)</returns>
        /// Author: PNNHai
        /// Date
        Task<IEnumerable<TRespondDto>> ReadExcelFileAsync(IFormFile importFile, string sheetUsed, string workingObjectTable);

        /// <summary>
        /// Hàm thực hiện xác nhận nhập khẩu file vào db
        /// </summary>
        /// <param name="workingTable">Tên bảng thực hiện import</param>
        /// <param name="confirmType">Trạng thái người dùng có xác nhận import hay không (0: ko; 1: có)</param>
        /// Author: PNNHai
        /// Date
        Task ConfirmImport(string workingTable, ConfirmType confirmType);
    }
}
