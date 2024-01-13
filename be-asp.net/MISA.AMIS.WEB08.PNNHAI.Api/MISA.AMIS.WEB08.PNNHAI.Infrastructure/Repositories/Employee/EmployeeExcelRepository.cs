using AutoMapper;
using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public class EmployeeExcelRepository : ExcelExportRepository<Employee, EmployeeModel>, IEmployeeExcelRepository
    {
        public EmployeeExcelRepository(IEmployeeRepository employeeRepository, IMapper mapper) : base(employeeRepository, mapper)
        {
        }
    }
}
