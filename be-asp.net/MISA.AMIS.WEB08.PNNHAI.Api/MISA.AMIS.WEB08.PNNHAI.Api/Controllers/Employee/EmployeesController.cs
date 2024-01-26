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
        /// <summary>
        /// Endpoint xuất khẩu file excel
        /// </summary>
        /// <param name="excelResquest">Param truyền vào để xuất fil excel</param>
        /// <returns>File excel</returns>
        [HttpPost("Export")]
        public async Task<IActionResult> ExportExcelFile([FromBody] ExcelExportRequestDto excelResquest)
        {
            var excelFile = await _employeeExcelService.ExportExcelFileAsync(excelResquest);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excel.xlsx");
        }

        /// <summary>
        /// Endpoint nhập khẩu file excel
        /// </summary>
        /// <param name="importFile">Form chứa dữ liệu nhập khẩu</param>
        /// <returns>Trạng thái status code</returns>
        [HttpPost("Import")]
        public async Task<IActionResult> ImportExcelFile(IFormFile? importFile)
        {
            var res = await _employeeExcelService.ReadExcelFileAsync(importFile, "DanhSachNhanVien", "Employee");
            return StatusCode(StatusCodes.Status200OK, res);
        }

        [HttpPost("Confirm-Import-Excel")]
        public async Task<IActionResult> Confirm(string workingTable, ConfirmType confirmType)
        {
            await _employeeExcelService.ConfirmImport(workingTable, confirmType);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Endpoint xuất khẩu file excel
        /// </summary>
        /// <param name="excelResquest">Param truyền vào để xuất fil excel</param>
        /// <returns>File excel</returns>
        [HttpPost("Dowload-File-Template")]
        public async Task<IActionResult> DowloadFileTemplate(string workingTable)
        {
            var excelFile = await _employeeExcelService.DowloadTemplateFile(workingTable);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "template.xlsx");
        }
        #endregion
    }
}
