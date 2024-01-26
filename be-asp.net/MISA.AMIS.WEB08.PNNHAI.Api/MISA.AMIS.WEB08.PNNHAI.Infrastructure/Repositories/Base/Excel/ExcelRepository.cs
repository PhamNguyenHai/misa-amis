using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MISA.AMIS.WEB08.PNNHAI.Core;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public abstract class ExcelRepository<TRespondDto, TEntity, TModel> 
        : ExcelExportRepository<TEntity, TModel>, IExcelRepository<TRespondDto, TEntity>
        where TRespondDto : class, new()
        where TEntity : class, new()
    {
        protected readonly IMemoryCache _memoryCache;
        protected readonly IUnitOfWork _uow;

        #region Constructor
        public ExcelRepository(IReadOnlyRepository<TEntity, TModel> readOnlyRepository,
            IMapper mapper, IMemoryCache memoryCache, IUnitOfWork unitOfWork) 
            : base(readOnlyRepository, mapper)
        {
            _memoryCache = memoryCache;
            _uow = unitOfWork;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thực hiện dowload file mẫu để import ứng với nghiệp vụ
        /// </summary>
        /// <param name="filePath">đường dẫn của file cần lấy</param>
        /// <returns>Mangr byte của file mẫu</returns>
        /// Author: PNNHai
        /// Date
        public async Task<byte[]> DowloadTemplateFile(string filePath)
        {
            // Kiểm tra xem tệp có tồn tại không
            if (!File.Exists(filePath))
            {
                throw new ValidateException(Core.Resources.AppResource.FileNotFoundError);
            }

            // Trả về tệp Excel cho client
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            return fileBytes;
        }

        /// <summary>
        /// Hàm thực hiện set dữ liệu vào cache
        /// </summary>
        /// <param name="key">key muốn set</param>
        /// <param name="value">giá trị muốn set</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        public void SetCache(string key, object value)
        {
            //throw new NotImplementedException();
            TimeSpan slidingExpiration = TimeSpan.FromMinutes(3);
            _memoryCache.Set(key, value, slidingExpiration);
        }

        /// <summary>
        /// Hàm thực hiện lấy dữ liệu từ cache thông qua key
        /// </summary>
        /// <param name="key">key của dữ liệu muốn lấy</param>
        /// <returns>Dữ liệu muốn lấy</returns>
        /// Author: PNNHai
        /// Date:
        public object GetCache(string key)
        {
            var data = _memoryCache.Get<object>(key);
            return data;
        }

        /// <summary>
        /// Hàm thực hiện lấy danh sách đối tượng từ cache thông qua table name
        /// </summary>
        /// <param name="tableName">tên bảng muốn lấy từ cache</param>
        /// <returns>danh sách đối tượng</returns>
        /// Author: PNNHai
        /// Date:
        public List<object> GetListObjectByTableName(string tableName)
        {
            var data = _memoryCache.Get<List<object>>(tableName);
            return data;
        }

        /// <summary>
        /// Hàm thực hiện thêm nhiều
        /// </summary>
        /// <param name="entities">danh sách muốn thêm</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        public async Task InsertRangeAsync(List<TEntity> entities)
        {
            try
            {
                string tableName = typeof(TEntity).Name;
                string storedProcedureName = $"Proc_{tableName}_InsertRange";

                // Chuyển list data sang dạng json truyền lên stored để insert
                var entitiesJsonString = JsonSerializer.Serialize(entities);
                var param = new DynamicParameters();
                param.Add($"i_{tableName}s", entitiesJsonString);

                _uow.BeginTransaction();

                await _uow.Connection.ExecuteAsync(storedProcedureName, param,
                    commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

                _uow.Commit();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Hàm thực hiện xóa dữ liệu cache bằng key
        /// </summary>
        /// <param name="key">key của dữ liệu muốn xóa</param>
        /// Author: PNNHai
        /// Date:
        public void RemoveByKeyCache(string key)
        {
            _memoryCache.Remove(key);
        }
        #endregion
    }
}
