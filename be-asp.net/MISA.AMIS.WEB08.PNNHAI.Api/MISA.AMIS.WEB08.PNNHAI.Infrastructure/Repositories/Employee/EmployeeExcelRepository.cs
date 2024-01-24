using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public class EmployeeExcelRepository : ExcelRepository<EmployeeExcelImportRespondDto, Employee, EmployeeModel>, IEmployeeExcelRepository
    {
        public EmployeeExcelRepository(IEmployeeRepository employeeRepository, IMapper mapper, IMemoryCache memoryCache, IUnitOfWork unitOfWork) 
            : base(employeeRepository, mapper, memoryCache, unitOfWork)
        {
        }
    }
}
