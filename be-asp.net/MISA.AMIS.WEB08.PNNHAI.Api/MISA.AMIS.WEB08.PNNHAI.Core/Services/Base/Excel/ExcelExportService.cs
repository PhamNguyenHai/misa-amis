using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public abstract class ExcelExportService : IExcelExportService
    {
        #region Fields
        protected readonly IExcelExportRepository _excelExportRepository;
        #endregion

        #region Constructor
        public ExcelExportService(IExcelExportRepository excelExportRepository)
        {
            _excelExportRepository = excelExportRepository;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Hàm thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="excelExportRequest">dữ liệu cần thiết để xuất ra file</param>
        /// <returns>mảng byte</returns>
        /// Author: PNNHai
        /// Date:
        public Task<byte[]> ExportExcelFileAsync(ExcelExportRequestDto excelExportRequest)
        {
            var data = _excelExportRepository.ExportExcelFileAsync(excelExportRequest);
            return data;
        } 
        #endregion
    }
}
