using AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class EmployeeExcelService : ExcelService<EmployeeExcelImportRespondDto, EmployeeCreateDto, Employee>, IEmployeeExcelService
    {
        #region Fields
        protected readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Constructor
        public EmployeeExcelService(IEmployeeExcelRepository employeeExcelRepository, 
            IExcelImportTemplateSettingRepository _excelImportTemplateSettingRepository, IDepartmentRepository _departmentRepository, 
            IEmployeeRepository employeeRepository, IMapper mapper)
            : base(employeeExcelRepository, _excelImportTemplateSettingRepository, _departmentRepository, mapper)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thực hiện validate các dữ liệu cụ thể hơn
        /// </summary>
        /// <param name="employeeExcelResults">Dữ liệu được đọc từ file excel</param>
        /// <param name="employeeObject">Đối tượng đang xử lý</param>
        /// <param name="rowObject">Dữ liệu của dòng đang xử lý</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        protected override async Task ValidateObjectDetail(List<EmployeeExcelImportRespondDto> employeeExcelResults, 
            EmployeeCreateDto employeeObject, EmployeeExcelImportRespondDto rowObject)
        {
            // Kiểm tra email có hợp lệ không 
            if (!string.IsNullOrEmpty(employeeObject.Email) && !IsEmail(employeeObject.Email))
            {
                rowObject.Errors.Add(Core.Resources.AppResource.EmailWrongFormat);
            }

            // Kiểm tra ngày sinh có lớn hơn ngày hiện tại không 
            if (employeeObject.DateOfBirth.HasValue && IsDateGreaterThanToday(employeeObject.DateOfBirth.Value))
            {
                rowObject.Errors.Add(Core.Resources.AppResource.DateOfBirthNoMoreThanCurrent);
            }

            // Kiểm tra ngày cấp có lớn hơn ngày hiện tại không 
            if (employeeObject.IdentityIssuedDate.HasValue && IsDateGreaterThanToday(employeeObject.IdentityIssuedDate.Value))
            {
                rowObject.Errors.Add(Core.Resources.AppResource.IdentityIssuedDateNoMoreThanCurrent);
            }

            // Kiểm tra số điện thoại có hợp lệ không 
            if (!string.IsNullOrEmpty(employeeObject.PhoneNumber) && !IsPhoneNumber(employeeObject.PhoneNumber))
            {
                rowObject.Errors.Add(Core.Resources.AppResource.PhoneNumberWrongFormat);
            }

            // Kiểm tra xem sdt cố định hợp lệ không
            if (!string.IsNullOrEmpty(employeeObject.LandlineNumber) && !IsPhoneNumber(employeeObject.LandlineNumber))
            {
                rowObject.Errors.Add(Core.Resources.AppResource.LandlineNumberWrongFormat);
            }

            // Kiểm tra xem thông tin mã nhập vào có thỏa mãn không
            if (!string.IsNullOrEmpty(employeeObject.EmployeeCode) && !IsEmployeeCode(employeeObject.EmployeeCode))
            {
                rowObject.Errors.Add(Core.Resources.AppResource.EmployeeCodeWrongFormat);
            }
            // Nếu mã code thỏa mãn thì mới tiến hành kiểm tra tiếp
            else
            {
                // Validate check mã trùng trong hệ thống
                var employee = await _employeeRepository.FindByCodeAsync(employeeObject.EmployeeCode);
                if (employee != null)
                {
                    rowObject.Errors.Add(string.Format(Core.Resources.AppResource.EmployeeCodeContainsInDB, employeeObject.EmployeeCode));
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
                                employeeExcelResults[index].Errors.Add(string.Format(Core.Resources.AppResource.EmployeeCodeContainsInImportFile, employeeObject.EmployeeCode));
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
                                employeeExcelResults[index].Errors.Add(string.Format(Core.Resources.AppResource.EmployeeCodeContainsInImportFile, employeeObject.EmployeeCode));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Kiểm tra xem mã có đúng định dạng không
        /// </summary>
        /// <param name="input">Mã truyền vào</param>
        /// <returns>true: nếu đúng định dạng; false nếu sai định dạng</returns>
        /// Author: PNNHai
        /// Date:
        protected bool IsEmployeeCode(string input)
        {
            if (input.StartsWith("NV-"))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
