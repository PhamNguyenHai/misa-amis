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
        Task<IEnumerable<TRespondDto>> ReadExcelFileAsync(IFormFile importFile, string sheetUsed, string workingObjectTable);
    }
}
