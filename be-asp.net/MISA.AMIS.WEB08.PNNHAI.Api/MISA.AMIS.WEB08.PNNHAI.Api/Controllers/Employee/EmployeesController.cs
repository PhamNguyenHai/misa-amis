﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.WEB08.PNNHAI.Core;

namespace MISA.AMIS.WEB08.PNNHAI.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class EmployeesController
        : BaseWithCodeController<Employee, EmployeeDto, EmployeeCreateDto, EmployeeUpdateDto>
    {
        #region Fields
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeExcelService _employeeExcelService;
        private readonly IEmployeeStatisticalService _employeeStatisticalService;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeService employeeService, 
            IEmployeeExcelService employeeExcelService, IEmployeeStatisticalService employeeStatisticalService)
            : base(employeeService)
        {
            _employeeService = employeeService;
            _employeeExcelService = employeeExcelService;
            _employeeStatisticalService = employeeStatisticalService;
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
        public async Task<IActionResult> ImportExcelFile(Guid userId, IFormFile? importFile, 
            string workingTable, string importSheetName)
        {
            //var res = await _employeeExcelService.ReadExcelFileAsync(userId, importFile, "DanhSachNhanVien", "Employee");
            var res = await _employeeExcelService.ReadExcelFileAsync(userId, importFile, importSheetName, workingTable);
            return StatusCode(StatusCodes.Status200OK, res);
        }

        [HttpPost("Confirm-Import-Excel")]
        public async Task<IActionResult> Confirm(Guid userId, string workingTable, ConfirmType confirmType)
        {
            await _employeeExcelService.ConfirmImport(userId, workingTable, confirmType);
            return StatusCode(StatusCodes.Status200OK);
        }

        /// <summary>
        /// Endpoint xuất khẩu file excel
        /// </summary>
        /// <param name="excelResquest">Param truyền vào để xuất fil excel</param>
        /// <returns>File excel</returns>
        [HttpGet("Dowload-File-Template")]
        public async Task<IActionResult> DowloadFileTemplate(string workingTable)
        {
            var excelFile = await _employeeExcelService.DowloadTemplateFile(workingTable);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "template.xlsx");
        }

        /// <summary>
        /// Hàm thực hiện thống kê dữ liệu thông qua thuộc tính
        /// </summary>
        /// <param name="propertyKey">thuộc tính cần thống kê theo</param>
        /// <returns>Danh sách dữ liệu thống kê</returns>
        [HttpGet("Statistical")]
        public async Task<IActionResult> StatisticalByPropetyKey(string propertyKey)
        {
            var statisticalResult = await _employeeStatisticalService.StatisticalByPropertyKey(propertyKey);
            return StatusCode(StatusCodes.Status200OK, statisticalResult);
        }
        #endregion
    }
}
