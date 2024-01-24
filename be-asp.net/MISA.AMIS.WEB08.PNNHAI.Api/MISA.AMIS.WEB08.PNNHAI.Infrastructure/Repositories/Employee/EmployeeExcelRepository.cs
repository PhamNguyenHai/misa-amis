using AutoMapper;
using Microsoft.AspNetCore.Http;
using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public class EmployeeExcelRepository : ExcelRepository<EmployeeExcelImportRespondDto, Employee, EmployeeModel>, IEmployeeExcelRepository
    {
        public EmployeeExcelRepository(IEmployeeRepository employeeRepository, IMapper mapper) : base(employeeRepository, mapper)
        {
        }
    }
}
