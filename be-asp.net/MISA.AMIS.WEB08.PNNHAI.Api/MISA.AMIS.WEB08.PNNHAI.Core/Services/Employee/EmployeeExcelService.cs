using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class EmployeeExcelService : ExcelExportService, IEmployeeExcelService
    {
        public EmployeeExcelService(IEmployeeExcelRepository employeeExcelRepository) : base(employeeExcelRepository)
        {
        }
    }
}
