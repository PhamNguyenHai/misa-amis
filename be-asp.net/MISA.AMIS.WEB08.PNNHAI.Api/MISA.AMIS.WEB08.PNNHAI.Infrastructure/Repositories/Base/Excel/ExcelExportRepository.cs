using MISA.AMIS.WEB08.PNNHAI.Core;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LicenseContext = OfficeOpenXml.LicenseContext;
using AutoMapper;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public abstract class ExcelExportRepository<TEntity, TModel> : IExcelExportRepository
    {
        #region Fields
        private readonly IReadOnlyRepository<TEntity, TModel> _readOnlyRepository;
        protected readonly IMapper _mapper;

        public virtual string Title { get; set; } = "Danh sách bản ghi";
        public virtual string Sheet { get; set; } = "DanhSachNhanVien";
        #endregion

        #region Constructor
        public ExcelExportRepository(IReadOnlyRepository<TEntity, TModel> readOnlyRepository, IMapper mapper)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thực hiện xuất dữ liệu ra file excel
        /// </summary>
        /// <param name="excelExportRequest">dữ liệu cần để thực hiện xuất file excel</param>
        /// <returns>mảng byte chứa dữ liệu xuất</returns>
        /// Author: PNNHai
        /// Date:
        public Task<byte[]> ExportExcelFileAsync(ExcelExportRequestDto excelExportRequest)
        {
            if (!string.IsNullOrEmpty(excelExportRequest.Title))
            {
                 Title = excelExportRequest.Title;
            }
            if (!string.IsNullOrEmpty(excelExportRequest.Sheet))
            {
                Sheet = excelExportRequest.Sheet;
            }

            switch (excelExportRequest.ExportType)
            {
                // Xử lý trường hợp xuất tất cả bản ghi
                case ExportType.ExportAll:
                    return ExportExcelFileWithAllAsync(excelExportRequest.Columns);
                // Xử lý trường hợp xuất với điều kiện lọc
                case ExportType.ExportWithFilterCondition:
                    var filterInput = _mapper.Map<FilterInput>(excelExportRequest);
                    filterInput.CurrentPage = -1;
                    return ExportExcelFileWithFilterConditiondAsync(filterInput, excelExportRequest.Columns);
                // Trả về exception nếu lựa chọn khác
                default:
                    throw new ValidateException(Core.Resources.AppResource.WrongSelectionError);
            }
        }

        /// <summary>
        /// Hàm thực hiện xuất dữ liệu ra file excel với tất cả dữ liệu
        /// </summary>
        /// <param name="columns">danh sách column muốn xuất</param>
        /// <returns>mảng byte chứa dữ liệu sau khi xuất</returns>
        private async Task<byte[]> ExportExcelFileWithAllAsync(IEnumerable<ExcelExportColumn> columns)
        {
            var data = await _readOnlyRepository.GetAllAsync();

            var result = CreateExcelAsync(data, columns);
            return result;
        }

        /// <summary>
        /// Hàm thực hiện xuất dữ liệu thỏa mãn điều kiện lọc ra file excel
        /// </summary>
        /// <param name="filterInput">điều kiện lọc</param>
        /// <param name="columns">danh sách column muốn xuất</param>
        /// <returns>mảng byte chứa dữ liệu</returns>
        /// Author: PNNHai
        /// Date:
        private async Task<byte[]> ExportExcelFileWithFilterConditiondAsync(FilterInput filterInput, IEnumerable<ExcelExportColumn> columns)
        {
            var data = await _readOnlyRepository.FilterPagingAsync(filterInput);
            var result = CreateExcelAsync(data.Data, columns);
            return result;
        }

        /// <summary>
        /// Hàm thực hiện tạo file excel xuất
        /// </summary>
        /// <param name="data">dữ liệu muốn xuất</param>
        /// <param name="Columns">danh sách các cột muốn xuất</param>
        /// <returns>mảng byte của file muốn xuất</returns>
        /// Author: PNNHai
        /// Date:
        private byte[] CreateExcelAsync(IEnumerable<TModel> data, IEnumerable<ExcelExportColumn> Columns)
        {
            var columnsToList = Columns.ToList();
            var stream = new MemoryStream();
            // Giấy phép
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add(Sheet);

                var startRow = 3;
                var row = startRow;
                var totalColumns = columnsToList.Count();

                sheet.Cells[1, 1].Value = Title;
                // Gộp các ô lại để hiển thị tiêu đề
                sheet.Cells[1, 1, 1, totalColumns + 1].Merge = true;
                sheet.Cells[1, 1, 1, totalColumns + 1].Style.Font.Size = 18;
                sheet.Cells[1, 1, 1, totalColumns + 1].Style.Font.Bold = true;
                sheet.Cells[1, 1, 1, totalColumns + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // thiết lập các tên cột-------------------------------------------------------------------------------
                sheet.Cells[startRow, 1].Value = "STT";
                sheet.Cells[startRow, 1].Style.Fill.SetBackground(Color.Gray);
                sheet.Cells[startRow, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells[startRow, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells[startRow, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                sheet.Cells[startRow, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                for (int i = 0; i < columnsToList.Count(); i++)
                {
                    var horizontalAlign = ExcelHorizontalAlignment.Left;
                    switch (columnsToList[i].Align)
                    {
                        case TextAlign.Center:
                            horizontalAlign = ExcelHorizontalAlignment.Center;
                            break;
                        case TextAlign.Right:
                            horizontalAlign = ExcelHorizontalAlignment.Right;
                            break;
                        default:
                            break;
                    }
                    //sheet.Cells[startRow, i + 2].Style.WrapText = true;
                    sheet.Row(startRow).Height = 25;
                    sheet.Cells[startRow, i + 2].Value = columnsToList[i].Title;
                    sheet.Cells[startRow, i + 2].Style.HorizontalAlignment = horizontalAlign;
                    sheet.Cells[startRow, i + 2].Style.Fill.SetBackground(Color.Gray);
                    sheet.Cells[startRow, i + 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[startRow, i + 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[startRow, i + 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[startRow, i + 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                };

                // thiết lập giá trị cho từng hàng cột-------------------------------------------------------------------------------

                var index = 1;
                foreach (var item in data)
                {
                    //Cài đặt các hàng------------------------------------------------------------------------------------
                    var rowStyle = sheet.Cells[startRow + 1, 1, startRow + 1, totalColumns + 1];
                    rowStyle.Style.VerticalAlignment = ExcelVerticalAlignment.Center; // Căn giữa dọc cho các hàng
                    rowStyle.Style.Font.Size = 11;
                    rowStyle.Style.Font.Color.SetColor(Color.Black);
                    rowStyle.Style.Font.Name = "Arial";
                    //Border cho các ô
                    var rowBodyBorder = rowStyle.Style.Border;
                    rowBodyBorder.BorderAround(ExcelBorderStyle.Medium);
                    rowBodyBorder.Top.Style = ExcelBorderStyle.Thin;
                    rowBodyBorder.Bottom.Style = ExcelBorderStyle.Thin;
                    rowBodyBorder.Left.Style = ExcelBorderStyle.Thin;
                    rowBodyBorder.Right.Style = ExcelBorderStyle.Thin;
                    //Đặt chiều cao cho ô
                    sheet.Row(startRow + 1).Height = 25;
                    //căn giữa cột đầu tiên
                    sheet.Cells[startRow + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[startRow + 1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center; // Căn giữa dọc

                    //set giá trị cho các ô-------------------------------------------------------------
                    sheet.Cells[startRow + 1, 1].Value = index;
                    for (int i = 0; i < totalColumns; i++)
                    {
                        var propertyName = columnsToList[i].ColumnKey;
                        PropertyInfo propertyInfo = item.GetType().GetProperty(propertyName);
                        var type = columnsToList[i].FormatType;
                        var align = columnsToList[i].Align;
                        //sheet.Cells[startRow + 1, i + 2].Style.WrapText = true;
                        setValueColumn(sheet, startRow + 1, i + 2, propertyInfo?.GetValue(item), type, align);
                    }
                    index++;
                    startRow++; //Tăng vị trí hàng tiếp theo
                }

                // Tự động căn chỉnh kích thước của column
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                package.Save();
            }
            return stream.ToArray();
        }

        /// <summary>
        /// Hàm thực hiện set giá trị cho ô
        /// </summary>
        /// <param name="worksheet">sheet thực hiện xuất</param>
        /// <param name="row">dòng đang xử lý</param>
        /// <param name="col">cột đang xử lý</param>
        /// <param name="value">dữ liệu đang xử lý</param>
        /// <param name="type">loại dữ liệu của cột đó</param>
        /// <param name="align">căn chỉnh</param>
        /// Author: PNNHai
        /// Date:
        private void setValueColumn(ExcelWorksheet worksheet, int row, int col, object? value, FormatType? type, TextAlign? align)
        {
            var horizontalAlign = ExcelHorizontalAlignment.Left;

            switch (align)
            {
                case TextAlign.Center:
                    horizontalAlign = ExcelHorizontalAlignment.Center;
                    break;
                case TextAlign.Right:
                    horizontalAlign = ExcelHorizontalAlignment.Right;
                    break;
                default:
                    break;
            }

            worksheet.Cells[row, col].Style.HorizontalAlignment = horizontalAlign;
            worksheet.Cells[row, col].Value = value;
            switch (type)
            {
                case FormatType.Date:
                    if (value is DateTime dateValue)
                    {
                        worksheet.Cells[row, col].Value = dateValue.ToString("dd/MM/yyyy");
                    }
                    break;
                case FormatType.Gender:
                    var fieldInfo = value.GetType().GetField(value.ToString());//lấy thông tin về trường enum tương ứng với giá trị value
                    var nameGender = "";
                    if (fieldInfo != null)
                    {
                        //để lấy danh sách các thuộc tính tùy chỉnh trên trường đó. có mỗi Description
                        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        if (attributes.Length > 0)
                        {
                            nameGender = attributes[0].Description.ToString();
                        }
                    }
                    worksheet.Cells[row, col].Value = nameGender;
                    break;
                default:
                    worksheet.Cells[row, col].Value = value;
                    break;
            }
        }
        #endregion
    } 
}
