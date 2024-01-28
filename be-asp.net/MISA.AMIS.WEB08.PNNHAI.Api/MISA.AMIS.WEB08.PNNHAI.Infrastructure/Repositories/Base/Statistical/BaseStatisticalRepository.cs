using Dapper;
using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public abstract class BaseStatisticalRepository : IBaseStatisticalRepository
    {
        #region Fields
        protected readonly IUnitOfWork _uow;
        public abstract string EntityName { get; protected set; }
        #endregion

        #region Constructor
        public BaseStatisticalRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thực hiện thống kê dữ liệu theo tên thuộc tính
        /// </summary>
        /// <param name="propertyKey">Thuộc tính cần thống kê theo</param>
        /// <returns>Danh sách dữ liệu thống kê</returns>
        public async Task<IEnumerable<StatisticalDto>> GetStatisticalByPropertyKeyAsync(string propertyKey)
        {
            string storedProcedureName = $"Proc_{EntityName}_Statistical";

            var param = new DynamicParameters();
            param.Add($"i_StatisticalField", propertyKey);

            var result = await _uow.Connection.QueryAsync<StatisticalDto>(storedProcedureName, param,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return result;
        } 
        #endregion
    }
}
