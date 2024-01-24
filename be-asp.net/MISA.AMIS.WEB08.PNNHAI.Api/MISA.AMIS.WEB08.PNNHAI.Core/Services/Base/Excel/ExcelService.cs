﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Reflection;
using static OfficeOpenXml.ExcelErrorValue;
using System.Drawing;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public abstract class ExcelService<TRespondDto, TEntityCreateDto> : ExcelExportService, IExcelService<TRespondDto> where TRespondDto : ExcelImportRespondedDto, new() where TEntityCreateDto: new()
    {
        #region Fields
        protected readonly IExcelRepository<TRespondDto> _excelRepository;
        protected readonly IExcelImportTemplateSettingRepository _excelImportTemplateSettingRepository;
        protected List<TEntityCreateDto> _validEntityToCreate;
        protected readonly IDepartmentRepository _departmentRepository;
        #endregion

        #region Constructor
        public ExcelService(IExcelRepository<TRespondDto> excelRepository, IExcelImportTemplateSettingRepository excelImportTemplateSettingRepository, IDepartmentRepository departmentRepository)
            : base(excelRepository)
        {
            _excelRepository = excelRepository;
            _excelImportTemplateSettingRepository = excelImportTemplateSettingRepository;
            _departmentRepository = departmentRepository;

            _validEntityToCreate = new List<TEntityCreateDto>();
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<TRespondDto>> ReadExcelFileAsync(IFormFile importFile, string sheetUsed, string workingObjectTable)
        {
            // Kiểm tra xem file có được gửi lên không 
            if (importFile == null || importFile.Length <= 0)
            {
                throw new ValidateException(Core.Resources.AppResource.SendWithoutFileError);
            }
            // Nếu có file thì kiểm tra xem có phải file excel không
            else
            {
                string fileExtension = Path.GetExtension(importFile.FileName);
                if (fileExtension != ".xlsx")
                {
                    // Xử lý khi tệp tin không phải là Excel
                    throw new ValidateException(Core.Resources.AppResource.SendWithoutExcelFileError);

                }
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var stream = new MemoryStream())
            {
                await importFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[sheetUsed];
                    if (worksheet == null)
                    {
                        throw new ValidateException("Bạn đã gửi tệp với sheet không thỏa mãn lên !");
                    }

                    //// Lấy danh sách các ô có giá trị
                    //var cellsWithData = worksheet.Cells.Where(cell => cell.Value != null);
                    //// Xác định kích thước thực tế của dữ liệu
                    //int rowCount = cellsWithData.Max(cell => cell.End.Row);
                    //int columnCount = cellsWithData.Max(cell => cell.End.Column);

                    var settingTemplate = await _excelImportTemplateSettingRepository.GetExcelImportTemplateSettingAsync(workingObjectTable);
                    var templateColumns = settingTemplate.ImportColumns;

                    // Kiểm tra xem file có đúng định dạng các cột không
                    ValidateFormatPostedExcelFile(worksheet, settingTemplate, templateColumns);

                    // Lấy ra dữ liệu setting template cho việc đọc dữ liệu
                    var columnCount = worksheet.Dimension.End.Column;
                    var rowCount = worksheet.Dimension.End.Row;

                    // Lấy ra các cột trong template

                    // Build danh sách các đối tượng đọc từ file excel
                    var objectResults = await BuildListObject(worksheet, columnCount, rowCount, settingTemplate, templateColumns);
                    return objectResults;
                }
            }
        }

        /// <summary>
        /// Hàm thực hiện validate xem mẫu định dạng của file excel truyền lên có đúng định dạng không
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="columnCount"></param>
        /// <param name="settingTemplate"></param>
        /// <param name="templateColumns"></param>
        /// <exception cref="ValidateException"></exception>
        private void ValidateFormatPostedExcelFile(ExcelWorksheet worksheet, ImportFileTemplate settingTemplate, ICollection<ImportColumn> templateColumns)
        {
            // Thực hiện kiểm tra tệp có đúng mẫu không 
            // Kiểm tra xem có cột/dòng nào trong file chưa 
            if (worksheet.Dimension == null)
            {
                throw new ValidateException("Tệp nhập khẩu gửi lên không có dữ liệu !");
            }

            var columnCount = worksheet.Dimension.End.Column;
            var rowCount = worksheet.Dimension.End.Row;

            // Nếu dimension có thì kiểm tra tiếp dòng và cột
            // Kiểm tra xem số lượng cột trong file và số lượng cột trong template có khớp không 
            if (templateColumns.Count > 0 && columnCount > 0 && templateColumns.Count != columnCount)
            {
                throw new ValidateException("Tệp nhập khẩu không đúng mẫu! Số lượng cột trong tệp khác số lượng quản lý !");
            }

            // Lấy ra vùng header để thực hiện
            //Số đầu tiên: Đại diện cho tọa độ hàng của ô bắt đầu trong phạm vi.
            //Số thứ hai: Đại diện cho tọa độ cột của ô bắt đầu trong phạm vi.
            //Số thứ ba: Đại diện cho tọa độ hàng của ô kết thúc trong phạm vi.
            //Số thứ tư: Đại diện cho tọa độ cột của ô kết thúc trong phạm vi.
            var headingRowIndex = settingTemplate.HeadingRowIndex;
            var rangeHeader = worksheet.Cells[headingRowIndex, 1, 1, columnCount];
            for (int i = 1; i < columnCount; i++)
            {
                var headerName = rangeHeader[headingRowIndex, i].Value.ToString().Replace("\n", "");
                var excelHeaderNameRemoveDiacritics = RemoveDiacritics(headerName).Trim();

                // Tìm trong template xem có tồn tại cột đó không
                var headerNameTemplate = templateColumns.Where(col => (
                    CompareStringsIgnoringPattern(RemoveDiacritics(col.ColumnTitle.Trim()), excelHeaderNameRemoveDiacritics))
                ).FirstOrDefault();

                // Nếu không tồn tại cột trong template -> exception -> break;
                if (headerNameTemplate == null)
                {
                    throw new ValidateException("Tệp nhập khẩu không đúng mẫu! Tồn tại cột không thỏa mãn dữ liệu quản lý !");
                }

                // Nếu thứ tự cột khác với thứ tự trong template quy định -> exception -> sau thứ tự cột
                else
                {
                    if(headerNameTemplate.ColumnIndex != i)
                        throw new ValidateException("Tệp nhập khẩu không đúng mẫu! Thứ tự các cột không chính xác !");
                }
            }
        }

        private async Task<IEnumerable<TRespondDto>> BuildListObject(ExcelWorksheet worksheet, int columnCount, int rowCount, ImportFileTemplate settingTemplate, ICollection<ImportColumn> templateColumns)
        {
            var objectResults = new List<TRespondDto>();
            // Bắt đầu từ hàng thứ startRowIndex, bỏ qua hàng tiêu đề
            for (int rowIndex = settingTemplate.HeadingRowIndex + 1; rowIndex <= rowCount; rowIndex++)
            {
                // Kiểm tra xem dòng đó có bị không có giá trị trong tất cả các ô không
                bool isRowEmpty = isExcelRowNullOrEmpty(worksheet, columnCount, rowIndex);
                if (isRowEmpty)
                    continue;

                var rowObject = new TRespondDto();
                var createObject = new TEntityCreateDto();
                rowObject = await BuildObject(worksheet, rowIndex, columnCount, templateColumns, createObject);

                objectResults.Add(rowObject);

                // Validate nghiệp vụ với object đã được build
                await ValidateObjectDetail(objectResults, createObject, rowObject);

                // Nếu dòng này thỏa mãn không có lỗi thì mới add createObject này vào danh sách hợp lệ
                if (rowObject.Errors.Count == 0)
                {
                    _validEntityToCreate.Add(createObject);
                }
            }
            return objectResults;
        }

        protected abstract Task ValidateObjectDetail(List<TRespondDto> objectResults, TEntityCreateDto createObject, TRespondDto rowObject);

        private bool isExcelRowNullOrEmpty(ExcelWorksheet worksheet, int columnCount, int rowIndex)
        {
            var rangeRow = worksheet.Cells[rowIndex, 1, 1, columnCount];

            for(int i= 1; i < columnCount; i++)
            {
                var cell = rangeRow[rowIndex, i].Value;
                if (cell != null)
                    return false;
            }

            return true;
        }

        private async Task<TRespondDto> BuildObject(ExcelWorksheet worksheet, int rowIndex, int columnCount, ICollection<ImportColumn> templateColumns, TEntityCreateDto createObject)
        {
            var rowObject = new TRespondDto();
            for(int columnIndex = 1; columnIndex <= columnCount; columnIndex++)
            {
                ImportColumn currentColumnTemplate = templateColumns.FirstOrDefault(col => col.ColumnIndex == columnIndex);

                var dataType = currentColumnTemplate != null ? (ColumnImportDataType)(currentColumnTemplate.ColumnDataType) : ColumnImportDataType.String;
                var cellValue = worksheet.Cells[rowIndex, columnIndex].Value;

                // Thực hiện validate với các trường yêu cầu bắt buộc
                RequiredValidate(rowObject, cellValue, currentColumnTemplate);

                if (cellValue == null)
                    continue;

                // Thực hiện validate với kiểu dữ liệu
                await ProcessCellValueByDataType(createObject, rowObject, dataType, cellValue, currentColumnTemplate);
            }
            return rowObject;
        }

        private void RequiredValidate(TRespondDto rowObject, object cellValue, ImportColumn currentColumnTemplate)
        {
            if (currentColumnTemplate.IsRequired == true && cellValue == null)
            {
                rowObject.Errors.Add(currentColumnTemplate.ColumnTitle + " không được phép để trống !");
            }
        }

        private async Task ProcessCellValueByDataType(TEntityCreateDto createObject, TRespondDto rowObject, ColumnImportDataType dataType, object cellValue, ImportColumn currentColumnTemplate)
        {
            var objectProperty = currentColumnTemplate.ColumnInsert;
            if(objectProperty != null)
            {
                var createObjectType = createObject.GetType();
                var rowObjectType = rowObject.GetType();
                var createObjectProperty = createObjectType.GetProperty(objectProperty);
                var rowObjectProperty = rowObjectType.GetProperty(objectProperty);
                // Kiểm tra xem thuộc tính đang xét đc định nghĩa trong db có tồn tại trong đối tượng không
                if (createObjectProperty != null && rowObjectProperty != null)
                {
                    switch (dataType)
                    {
                        // Validate và convert với những trường kiểu boolean đc định nghĩa trong db
                        case ColumnImportDataType.Boolean:
                            if (cellValue.ToString()?.Trim().ToLower() == "có")
                            {
                                createObjectProperty.SetValue(createObject, 1);
                            }
                            else if (cellValue.ToString()?.Trim().ToLower() == "không")
                            {
                                createObjectProperty.SetValue(createObject, 0);
                            }
                            else
                            {
                                rowObject.Errors.Add(currentColumnTemplate.ColumnTitle + " sai giá trị !");
                            }
                            rowObjectProperty.SetValue(rowObject, cellValue.ToString());
                            break;
                        // Validate và convert với những trường kiểu enum đc định nghĩa trong db
                        case ColumnImportDataType.Enum:
                            var enumName = currentColumnTemplate.ReferenceObjectName; // Enum khai báo trong Database
                                                                                      // Kiểm tra xem có Enum nào có tên như được khai báo hay không, nếu không khai báo thì bỏ qua:
                                                                                      // -> Nếu có thì thực hiện Lấy key từ Resource -> sau đó lấy giá trị và gán lại Property tương ứng" 
                            var enumNameStringContains = string.Format("MISA.AMIS.WEB08.PNNHAI.Core.{0}", enumName);
                            var enumType = Type.GetType(enumNameStringContains);
                            if (enumType != null)
                            {
                                // Lấy ra value của enum ứng với giá trị trong cellValue
                                var enumValue = Utility.GetEnumValueFromDescription(enumType, cellValue.ToString());
                                if(enumValue != null)
                                {
                                    createObjectProperty.SetValue(createObject, (int)enumValue);
                                }
                                else
                                {
                                    rowObject.Errors.Add(currentColumnTemplate.ColumnTitle + " có giá trị không tồn tại !");
                                }
                                rowObjectProperty.SetValue(rowObject, cellValue.ToString());
                            }
                            break;
                        // Validate và convert với những trường Reference đến table khác đc định nghĩa trong db
                        case ColumnImportDataType.ReferenceTable:
                            await ProcessCellValueWithTableReference(createObject, createObjectProperty, rowObject, cellValue, currentColumnTemplate);
                            rowObjectProperty.SetValue(rowObject, cellValue.ToString());
                            break;
                        default:
                            // Lấy ra kiểu của property đang xử lý
                            var propertyType = createObjectProperty.PropertyType;

                            if (propertyType == typeof(string))
                            {
                                createObjectProperty.SetValue(createObject, cellValue.ToString());
                                rowObjectProperty.SetValue(rowObject, cellValue.ToString());
                            }
                            else if (propertyType == typeof(double) || propertyType == typeof(double?))
                            {
                                var value = ConvertDouble(cellValue.ToString());
                                if(value != null)
                                {
                                    createObjectProperty.SetValue(createObject, value);
                                    rowObjectProperty.SetValue(rowObject, cellValue.ToString());
                                }
                                else
                                {
                                    rowObject.Errors.Add(currentColumnTemplate.ColumnTitle + " xung đột kiểu dữ liệu !");
                                }
                            }
                            else if(propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                            {
                                var value = ConvertDateTime(cellValue.ToString());
                                if (value != null)
                                {
                                    createObjectProperty.SetValue(createObject, value);
                                    var displayDateValue = value?.ToString("O");
                                    rowObjectProperty.SetValue(rowObject, displayDateValue);
                                }
                                else
                                {
                                    rowObject.Errors.Add(currentColumnTemplate.ColumnTitle + " có giá trị không hợp lệ !");
                                    rowObjectProperty.SetValue(rowObject, cellValue.ToString());
                                }
                            }
                            else
                            {
                                var value = Convert.ChangeType(cellValue, Nullable.GetUnderlyingType(propertyType));
                                createObjectProperty.SetValue(createObject, value);
                            }
                            break;
                    }
                }
            }
        }

        protected virtual async Task ProcessCellValueWithTableReference(TEntityCreateDto createObject, PropertyInfo createObjectProperty, TRespondDto rowObject,  object cellValue, ImportColumn currentColumnTemplate)
        {
            var tableRerefenceWithCurrentColumn = currentColumnTemplate.ReferenceObjectName;
            if (tableRerefenceWithCurrentColumn != null)
            {
                switch (tableRerefenceWithCurrentColumn)
                {
                    case "Department":
                        var departments = await _departmentRepository.GetAllAsync();
                        var departmentCellValue = departments.FirstOrDefault(department => department.DepartmentName == cellValue.ToString()?.Trim());

                        if (departmentCellValue == null)
                        {
                            rowObject.Errors.Add(currentColumnTemplate.ColumnTitle + " có giá trị không tồn tại !");
                            createObjectProperty.SetValue(createObject, null);

                        }
                        else
                        {
                            createObjectProperty.SetValue(createObject, departmentCellValue.DepartmentId);
                        }
                        break;
                    default : break;
                }
            }
        }

        protected DateTime? ConvertDateTime(string dateValue)
        {
            // Ngày tháng phải nhập theo định dạng (ngày/tháng/năm): 
            // VD hợp lệ: [25.04.2017] [02.04.2017] [2.4.2017] [25/04/2017] [5/12/2017] [15/2/2017] [25-04-2017]  [6-10-2017]  [16-5-2017] [09/26/2000 12:00:00 AM]  [09/26/2000 12:00:00 PM] 
            Regex ddmmyyyyValidRegex = new Regex(@"^([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$");
            Regex mmyyyyValidRegex = new Regex(@"^([0]?[1-9]|[1][0-2])[./-]([0-9]{4})$");
            Regex yyyyValidRegex = new Regex(@"^([0-9]{4})$");

            if (ddmmyyyyValidRegex.IsMatch(dateValue))
            {
                var dateSplit = dateValue.Split(new string[] { "/", ".", "-" }, StringSplitOptions.None);
                var day = int.Parse(dateSplit[0]);
                var month = int.Parse(dateSplit[1]);
                var year = int.Parse(dateSplit[2]);
                return new DateTime(year, month, day);
            }
            else if (mmyyyyValidRegex.IsMatch(dateValue))
            {
                var dateSplit = dateValue.Split(new string[] { "/", ".", "-" }, StringSplitOptions.None);
                var month = int.Parse(dateSplit[0]);
                var year = int.Parse(dateSplit[1]);
                return new DateTime(year, month, 1);
            }
            else if (yyyyValidRegex.IsMatch(dateValue))
            {
                return new DateTime(Convert.ToInt32(dateValue), 1, 1);
            }
            else
            {
                return null;
            }
        }

        protected double? ConvertDouble(string value)
        {
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Hàm thực hiện so sánh tương đối 2 chuỗi sau khi loại bỏ các ki tự đặc biệt
        /// Chuỗi lớn hơn có chứa giống chuỗi bé hơn không
        /// </summary>
        /// <param name="LargerString">Chuỗi lớn hơn</param>
        /// <param name="smallerString">Chuỗi bé hơn</param>
        /// <returns>true: Nếu có chứa | false: nếu không chứa</returns>
        /// Author: PNNHai
        /// Date
        protected bool CompareStringsIgnoringPattern(string LargerString, string smallerString)
        {
            string pattern = @"\(\*\)";
            string modifiedStr1 = Regex.Replace(LargerString, pattern, "");
            string modifiedStr2 = Regex.Replace(smallerString, pattern, "");

            return modifiedStr1.Contains(modifiedStr2);
        }

        /// <summary>
        /// Hàm chuyển các ký tự unicode thành ký tự không dấu, viết liền và viết thường (mục đích để compare gần đúng 2 chuỗi ký tự)
        /// </summary>
        /// <param name="text">Chuỗi ký tự</param>
        /// <returns>Chuỗi ký tự đã loại bỏ dấu và lowercase - phục vụ check map tương đối nội dung của text</returns>
        /// Author: PNNHai
        /// Date
        protected string RemoveDiacritics(string text)
        {
            var newText = string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
            return newText.Replace(" ", string.Empty).ToLower();
        }

        protected bool IsDateGreaterThanToday(DateTime inputDate)
        {
            return inputDate > DateTime.Today;
        }

        protected bool IsEmail(string input)
        {

            // Kiểm tra chuỗi có hợp lệ với pattern email không
            // Sử dụng Regular Expression
            // Đây là một pattern đơn giản, bạn có thể sử dụng pattern phức tạp hơn để kiểm tra email chính xác hơn
            string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(input);
        }

        protected bool IsPhoneNumber(string input)
        {
            // Kiểm tra chuỗi có chỉ chứa chữ số và có độ dài hợp lệ không
            // Sử dụng Regular Expression
            string phoneNumberPattern = @"^[0-9]+$";
            Regex regex = new Regex(phoneNumberPattern);
            return regex.IsMatch(input);
        }

        protected bool IsStringLengthGreaterThan(string input, int length)
        {
            return input.Length > length;
        }
        #endregion
    }
}
