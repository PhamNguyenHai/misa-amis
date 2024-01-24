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
    public abstract class BaseWithCodeRepository<TEntity, TModel> : BaseRepository<TEntity, TModel>, IBaseWithCodeRepository<TEntity, TModel>
        where TEntity : IHasKey, IHasCode
    {
        #region Fields
        public virtual string EntityCode { get; protected set; } = typeof(TEntity).Name + "Code";
        #endregion

        #region Constructor
        protected BaseWithCodeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm tìm kiếm đối tượng theo code
        /// </summary>
        /// <param name="code">mã của đối tượng</param>
        /// <returns>Đối tượng cần tìm</returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<TEntity?> FindByCodeAsync(string code)
        {

            string storedProcedureName = $"Proc_{EntityName}_GetByCode";

            var param = new DynamicParameters();
            param.Add($"i_{EntityCode}", code);

            var result = await _uow.Connection.QueryFirstOrDefaultAsync<TEntity?>(storedProcedureName, param,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return result;
        }

        /// <summary>
        /// Hàm lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<string> GetNewCodeAsync()
        {
            string storedProcedureName = $"Proc_{EntityName}_GetNewCode";

            var result = await _uow.Connection.QueryFirstOrDefaultAsync<string>(storedProcedureName,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return result;
        } 
        #endregion
    }
}
