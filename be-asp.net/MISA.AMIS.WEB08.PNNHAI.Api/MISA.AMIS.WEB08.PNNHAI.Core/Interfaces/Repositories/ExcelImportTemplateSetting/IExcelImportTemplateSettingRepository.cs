using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IExcelImportTemplateSettingRepository
    {
        /// <summary>
        /// Hàm thực hiện đọc dữ liệu để lấy ra cấu hình cho việc đọc file excel
        /// </summary>
        /// <param name="workingObjectTable">Bảng thực hiện import (trong csdl)</param>
        /// <returns>Dữ liệu để cấu hình cho việc đọc file excel</returns>
        /// Author: PNNHai
        /// Date: 
        Task<ImportFileTemplate> GetExcelImportTemplateSettingAsync(string workingObjectTable);
    }
}
