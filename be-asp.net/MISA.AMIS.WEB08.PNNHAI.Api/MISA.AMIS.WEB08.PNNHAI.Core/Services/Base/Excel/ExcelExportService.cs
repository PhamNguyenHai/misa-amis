using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class ExcelExportService : IExcelExportService
    {
        protected readonly IExcelExportRepository _excelExportRepository;
        public ExcelExportService(IExcelExportRepository excelExportRepository)
        {
            _excelExportRepository = excelExportRepository;
        }

        public Task<byte[]> ExportExcelFileAsync(ExcelExportRequestDto excelExportRequest)
        {
            var data = _excelExportRepository.ExportExcelFileAsync(excelExportRequest);
            return data;
        }
    }
}
