using Dapper;
using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public abstract class ReadOnlyRepository<TEntity, TModel> : IReadOnlyRepository<TEntity, TModel> where TEntity : IHasKey
    {
        #region Fields
        protected readonly IUnitOfWork _uow;

        // Tên bảng (Phương thức virtual có thể ghi đè lại. Mặc định lấy tên bảng là entity đó)
        public virtual string EntityName { get; protected set; } = typeof(TEntity).Name;

        // Id của bảng ứng với entity đó
        public virtual string EntityId { get; protected set; } = typeof(TEntity).Name + "Id";
        #endregion

        #region Constructor
        public ReadOnlyRepository(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm lấy toàn bộ danh sách đối tượng
        /// </summary>
        /// <returns>Danh sách đối tượng </returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            string storedProcedureName = $"Proc_{EntityName}_GetAll";
            var result = await _uow.Connection.QueryAsync<TModel>(storedProcedureName,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return result;
        }

        /// <summary>
        /// Hàm lấy đối tượng theo id
        /// </summary>
        /// <param name="id">Mã định danh đối tượng</param>
        /// <returns>Đối tượng cần lấy</returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<TModel> GetByIdAsync(Guid id)
        {
            var entity = await FindByIdAsync(id);
            if (entity == null)
                throw new NotFoundException(Core.Resources.AppResource.NotFoundById);
            return entity;
        }

        /// <summary>
        /// Tìm kiếm đối tượng theo id
        /// </summary>
        /// <param name="id">id đối tượng</param>
        /// <returns>Đối tượng cần tìm</returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<TModel?> FindByIdAsync(Guid id)
        {
            string storedProcedureName = $"Proc_{EntityName}_GetById";

            var param = new DynamicParameters();
            param.Add($"i_{EntityId}", id);

            var result = await _uow.Connection.QueryFirstOrDefaultAsync<TModel>(storedProcedureName, param,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return result;
        }

        /// <summary>
        /// Hàm lọc dữ liệu kết hợp phân trang, tìm kiếm và sắp xếp
        /// </summary>
        /// <param name="filterInput">Dữ liệu lọc ...</param>
        /// <returns>Kết quả phân trang</returns>
        /// <exception cref="ValidateException">Lựa chọn sai</exception>
        /// Author: PNNHai
        /// Date: 
        public async Task<FilterResult<TModel>> FilterPagingAsync(FilterInput filterInput)
        {
            // Khối lệnh sql để thực thi filter theo nhiều cột
            var filterSqlCommand = string.Empty;

            // Nếu input điều kiện filter cho cột > 1 cột -> gán khối lệnh sql filter
            if (filterInput.FilterColumns.Count() > 0)
            {
                // Duyệt từng điều kiện lọc ứng với từng cột (nếu có)
                foreach (var column in filterInput.FilterColumns)
                {
                    // Lấy ra type của cột tương ứng
                    var columnType = typeof(TEntity).GetProperty(column.ColumnName)?.PropertyType;
                    if (columnType != null)
                    {
                        // Với trường hợp type của column là kiểu số nguyên hoặc thực
                        if (columnType == typeof(double?) || columnType == typeof(int?))
                        {
                            // Xử lý điều kiện lọc cho kiểu số nguyên hoặc thực
                            filterSqlCommand += BuildNumericFilterCondition(column);
                        }

                        // Với trường hợp type của column là kiểu date time
                        else if (columnType == typeof(DateTime?))
                        {
                            // Xử lý điều kiện lọc cho kiểu DateTime
                            filterSqlCommand += BuildDateTimeFilterCondition(column);
                        }

                        // Với trường hợp type của column là kiểu chuỗi
                        else if (columnType == typeof(string))
                        {
                            // Xử lý điều kiện lọc cho kiểu chuỗi
                            filterSqlCommand += BuildStringFilterCondition(column);
                        }

                        // Với trường hợp type là kiểu lựa chọn (với các trường có sẵn)
                        else
                        {
                            // Xử lý điều kiện lọc cho kiểu lựa chọn
                            filterSqlCommand += BuildSelectionFilterCondition(column);
                        }
                    }
                    // Với trường hợp không tồn tại cột lọc
                    else
                    {
                        throw new ValidateException(Core.Resources.AppResource.WrongFilterColumn);
                    }
                }
            }

            string storedProcedureName = $"Proc_{EntityName}_Filter";

            var param = new DynamicParameters();
            param.Add("i_PageNumber", filterInput.CurrentPage);
            param.Add("i_PageSize", filterInput.PageSize);
            param.Add("i_Where", filterInput.SearchString);

            param.Add("i_SortColumn", filterInput.SortColumn);
            param.Add("i_IsSortDesc", filterInput.IsSortDesc);

            param.Add("i_FilterClause", filterSqlCommand);

            param.Add("o_TotalRecords", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var data = await _uow.Connection.QueryAsync<TModel>(storedProcedureName, param,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            int totalRecord = param.Get<int>("o_TotalRecords");
            // Nếu pageSize < 0 => totalPage = 1
            // Nếu pageSize >= 0 => Lấy totalPage
            int totalPage = filterInput.PageSize < 0 ? 1 : (int)Math.Ceiling(((double)totalRecord / filterInput.PageSize));
            var result = new FilterResult<TModel>(filterInput.CurrentPage, filterInput.PageSize, totalPage, totalRecord, data);
            return result;
        }

        /// <summary>
        /// Hàm thực hiện build chuỗi sql ứng với các column kiểu số(double, int)
        /// </summary>
        /// <param name="column">Column thực hiện filter</param>
        /// <returns>chuỗi sql thực hiện filter</returns>
        /// Author: PNNHai
        /// Date:
        private string BuildNumericFilterCondition(FilterColumn column)
        {
            // Lấy ra toán tử thực hiện
            var sqlCompareOperator = GetSqlCompareOperator(column.FilterType);
            return $" AND {column.ColumnName} {sqlCompareOperator} {column.FilterString}";
        }

        /// <summary>
        /// Hàm thực hiện build chuỗi sql ứng với các column kiểu date time
        /// </summary>
        /// <param name="column">Column thực hiện filter</param>
        /// <returns>chuỗi sql thực hiện filter</returns>
        /// Author: PNNHai
        /// Date:
        private string BuildDateTimeFilterCondition(FilterColumn column)
        {
            // Lấy ra toán tử thực hiện
            var sqlCompareOperator = GetSqlCompareOperator(column.FilterType);
            return $" AND {column.ColumnName} {sqlCompareOperator} '{column.FilterString}'";
        }

        /// <summary>
        /// Hàm thực hiện build sql string ứng với các column filter theo kiểu text
        /// </summary>
        /// <param name="column">column thực hiện filter</param>
        /// <returns>Chuỗi sql thực hiện</returns>
        /// <exception cref="ValidateException">Lựa chọn sai</exception>
        /// Author: PNNHai
        /// Date:
        private string BuildStringFilterCondition(FilterColumn column)
        {
            switch ((TextFilterType)column.FilterType)
            {
                case TextFilterType.StartWith:
                    return $" AND {column.ColumnName} LIKE '{column.FilterString}%'";
                case TextFilterType.EndWith:
                    return $" AND {column.ColumnName} LIKE '%{column.FilterString}'";
                case TextFilterType.Contain:
                    return $" AND {column.ColumnName} LIKE '%{column.FilterString}%'";
                case TextFilterType.NotContain:
                    return $" AND {column.ColumnName} NOT LIKE '%{column.FilterString}%'";
                case TextFilterType.Equal:
                    return $" AND {column.ColumnName} = '{column.FilterString}'";
                case TextFilterType.NotEqual:
                    return $" AND {column.ColumnName} <> '{column.FilterString}'";
                default:
                    throw new ValidateException(Core.Resources.AppResource.WrongSelectionError);
            }
        }

        /// <summary>
        /// Hàm thực hiện build chuỗi sql với trường hợp filter với các column ứng với kiểu selection
        /// </summary>
        /// <param name="column">column thực hiện filter</param>
        /// <returns>chuỗi sql thực hiện</returns>
        /// <exception cref="ValidateException">Lựa chọn sai</exception>
        /// Author: PNNHai
        /// Date:
        private string BuildSelectionFilterCondition(FilterColumn column)
        {
            
            switch ((SelectionFilterTye)column.FilterType)
            {
                case SelectionFilterTye.Equal:
                    {
                        
                        if (column.FilterString.Contains(","))
                        {
                            string filterStringSqlCommand = String.Empty;
                            filterStringSqlCommand = column.FilterString.Replace(",", $"' OR e.{column.ColumnName} = '");
                            //return $" e.{column.ColumnName} = '{column.FilterString}'";
                            return $" AND (e.{column.ColumnName} = '{filterStringSqlCommand}')";
                        }
                        return $" AND e.{column.ColumnName} = '{column.FilterString}'";
                    }
                case SelectionFilterTye.NotEqual:
                    {
                        if (column.FilterString.Contains(","))
                        {
                            string filterStringSqlCommand = String.Empty;
                            filterStringSqlCommand = column.FilterString.Replace(",", $"' AND e.{column.ColumnName} <> '");
                            //return $" e.{column.ColumnName} = '{column.FilterString}'";
                            return $" AND (e.{column.ColumnName} <> '{filterStringSqlCommand}')";
                        }
                        return $" AND e.{column.ColumnName} <> '{column.FilterString}'";
                    }
                default:
                    throw new ValidateException(Core.Resources.AppResource.WrongSelectionError);
            }
        }

        /// <summary>
        /// Hàm thực hiện lấy ra toán tử đã chọn ứng với kiểu lọc đã truyền
        /// </summary>
        /// <param name="filterType">Kiểu lọc (convert sang kiểu có thể so sánh)</param>
        /// <returns>toán tử ứng với kiểu so sánh</returns>
        /// <exception cref="ValidateException">Lựa chọn nhập vào sai</exception>
        /// Author: PNNHai
        /// Date:
        private string GetSqlCompareOperator(FilterType filterType)
        {
            switch ((ComparableFilterType)(int)filterType)
            {
                case ComparableFilterType.Greater:
                    return ">";
                case ComparableFilterType.Less:
                    return "<";
                case ComparableFilterType.GreaterOrEqual:
                    return ">=";
                case ComparableFilterType.LessOrEqual:
                    return "<=";
                case ComparableFilterType.Equal:
                    return "=";
                case ComparableFilterType.NotEqual:
                    return "<>";
                default:
                    throw new ValidateException(Core.Resources.AppResource.WrongSelectionError);
            }
        }
        #endregion
    }
}
