﻿using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class EmployeeExcelService : ExcelService<EmployeeExcelImportRespondDto, EmployeeCreateDto>, IEmployeeExcelService
    {
        protected readonly IEmployeeRepository _employeeRepository;

        public EmployeeExcelService(IEmployeeExcelRepository employeeExcelRepository, 
            IExcelImportTemplateSettingRepository _excelImportTemplateSettingRepository, IDepartmentRepository _departmentRepository, IEmployeeRepository employeeRepository) 
            : base(employeeExcelRepository, _excelImportTemplateSettingRepository, _departmentRepository)
        {
            _employeeRepository = employeeRepository;
        }

        protected override async Task ValidateObjectDetail(List<EmployeeExcelImportRespondDto> employeeExcelResults, EmployeeCreateDto employeeObject, EmployeeExcelImportRespondDto rowObject)
        {
            // Kiểm tra email có hợp lệ không 
            if (!string.IsNullOrEmpty(employeeObject.Email) && !IsEmail(employeeObject.Email))
            {
                rowObject.Errors.Add("Email sai định dạng !");
            }

            // Kiểm tra ngày sinh có lớn hơn ngày hiện tại không 
            if (employeeObject.DateOfBirth.HasValue && IsDateGreaterThanToday(employeeObject.DateOfBirth.Value))
            {
                rowObject.Errors.Add("Ngày sinh không được lớn hơn ngày hiện tại !");
            }

            // Kiểm tra ngày cấp có lớn hơn ngày hiện tại không 
            if (employeeObject.IdentityIssuedDate.HasValue && IsDateGreaterThanToday(employeeObject.IdentityIssuedDate.Value))
            {
                rowObject.Errors.Add("Ngày cấp CCCD không được lớn hơn ngày hiện tại !");
            }

            // Kiểm tra số điện thoại có hợp lệ không 
            if (!string.IsNullOrEmpty(employeeObject.PhoneNumber) && !IsPhoneNumber(employeeObject.PhoneNumber))
            {
                rowObject.Errors.Add("Số điện thoại di động sai định dạng !");
            }

            // Kiểm tra xem sdt cố định hợp lệ không
            if (!string.IsNullOrEmpty(employeeObject.LandlineNumber) && !IsPhoneNumber(employeeObject.LandlineNumber))
            {
                rowObject.Errors.Add("Số điện thoại cố định sai định dạng !");
            }

            // Kiểm tra xem thông tin mã nhập vào có thỏa mãn không
            if (!string.IsNullOrEmpty(employeeObject.EmployeeCode) && !IsEmployeeCode(employeeObject.EmployeeCode))
            {
                rowObject.Errors.Add("Mã nhân viên sai định dạng ! (Dạng NV-...)");
            }
            // Nếu mã code thỏa mãn thì mới tiến hành kiểm tra tiếp
            else
            {
                // Validate check mã trùng trong hệ thống
                var employee = await _employeeRepository.FindByCodeAsync(employeeObject.EmployeeCode);
                if (employee != null)
                {
                    rowObject.Errors.Add("Mã nhân viên " + employeeObject.EmployeeCode + " đã tồn tại trong hệ thống !");
                }

                // Validate check mã trùng ở các dòng trước trong dữ liệu đã đọc từ file excel

                if (employeeExcelResults.Count() > 1)
                {
                    if (employeeObject.EmployeeCode != null)
                    {
                        List<int> matchingIndexes = employeeExcelResults
                        .Select((employeeRow, index) => new { EmployeeRow = employeeRow, Index = index })
                        .Where(item => item.EmployeeRow.EmployeeCode?.ToLower().Trim() == employeeObject.EmployeeCode?.ToLower().Trim())
                        .Select(item => item.Index)
                        .ToList();


                        if (matchingIndexes.Count() == 2)
                        {
                            foreach (int index in matchingIndexes)
                            {
                                employeeExcelResults[index].Errors.Add("Mã nhân viên " + employeeObject.EmployeeCode + " đã bị trùng trong tệp nhập khẩu !");
                            }

                            var existEmployeeCodeObject = _validEntityToCreate.Where(
                            obj => obj.EmployeeCode?.ToLower().Trim() == employeeObject.EmployeeCode?.ToLower().Trim()).FirstOrDefault();

                            if (existEmployeeCodeObject != null)
                            {
                                _validEntityToCreate.Remove(existEmployeeCodeObject);
                            }
                        }

                        else if (matchingIndexes.Count() > 2)
                        {
                            List<int> otherMatchingIndexs = matchingIndexes.Skip(2).ToList();

                            foreach (int index in otherMatchingIndexs)
                            {
                                employeeExcelResults[index].Errors.Add("Mã nhân viên " + employeeObject.EmployeeCode + " đã bị trùng trong tệp nhập khẩu !");
                            }
                        }
                    }
                }
            }
        }

        protected bool IsEmployeeCode(string input)
        {
            if (input.StartsWith("NV-"))
            {
                return true;
            }
            return false;
        }
    }
}
