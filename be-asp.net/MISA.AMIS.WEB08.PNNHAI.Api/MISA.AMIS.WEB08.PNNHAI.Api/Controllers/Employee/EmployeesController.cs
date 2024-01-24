using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.WEB08.PNNHAI.Core;

namespace MISA.AMIS.WEB08.PNNHAI.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController
        : BaseWithCodeController<Employee, EmployeeDto, EmployeeCreateDto, EmployeeUpdateDto>
    {
        #region Fields
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeExcelService _employeeExcelService;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeService employeeService, IEmployeeExcelService employeeExcelService)
            : base(employeeService)
        {
            _employeeService = employeeService;
            _employeeExcelService = employeeExcelService;
        }
        #endregion

        #region Methods
        [HttpPost("Export")]
        public async Task<IActionResult> ExportExcelFile([FromBody] ExcelExportRequestDto excelResquest)
        {
            var excelFile = await _employeeExcelService.ExportExcelFileAsync(excelResquest);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excel.xlsx");
        }

        [HttpPost("Import")]
        public async Task<IActionResult> ImportExcelFile(IFormFile? importFile)
        {
            var res = await _employeeExcelService.ReadExcelFileAsync(importFile, "DanhSachNhanVien", "Employee");
            return StatusCode(StatusCodes.Status200OK, res);
        }
        #endregion
    }
}
