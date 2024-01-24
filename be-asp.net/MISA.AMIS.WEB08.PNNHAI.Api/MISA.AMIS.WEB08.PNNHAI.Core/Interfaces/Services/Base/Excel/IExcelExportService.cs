using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IExcelExportService
    {
        /// <summary>
        /// Xuất file excel
        /// </summary>
        /// <param name="excelExportRequest">request xuất file</param>
        /// <returns>Mảng byte</returns>
        /// Author: PNNHai
        /// Date:
        Task<byte[]> ExportExcelFileAsync(ExcelExportRequestDto excelExportRequest);
    }
}
