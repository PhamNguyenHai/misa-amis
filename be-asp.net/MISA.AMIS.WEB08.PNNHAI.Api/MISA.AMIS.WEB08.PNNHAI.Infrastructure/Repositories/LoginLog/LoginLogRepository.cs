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
    public class LoginLogRepository : ILoginLogRepository
    {
        #region Fields
        protected readonly IUnitOfWork _uow;
        #endregion

        #region Constructor
        public LoginLogRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        #region Methods
        public async Task<LoginLog?> FindLoginLogByIdAsync(Guid loginLogId)
        {
            string storedProcedureName = "Proc_loginlog_GetById";
            var loginLog = await _uow.Connection.QueryFirstOrDefaultAsync<LoginLog>(storedProcedureName,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return loginLog;
        }

        public async Task ChangeLogoutDateByLoginIdAsync(Guid loginLogId, DateTime logoutDate)
        {
            string storedProcedureName = "Proc_loginlog_ChangeLogoutDate";

            var parametters = new DynamicParameters();
            parametters.Add("i_LoginId", loginLogId);
            parametters.Add("i_LogoutDate", logoutDate);

            await _uow.Connection.ExecuteAsync(storedProcedureName, parametters,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
        }

        public async Task InsertLoginLogAsync(LoginLog loginLogToInsert)
        {
            string storedProcedureName = "Proc_loginlog_Insert";

            loginLogToInsert.CreatedDate = DateTime.Now;
            loginLogToInsert.CreatedBy = "admin";
            loginLogToInsert.ModifiedDate = DateTime.Now;
            loginLogToInsert.ModifiedBy = "admin";

            // Chuyển entity sang parametters để truyền vào procedure
            var parametters = CreateParamettersFromEntity(loginLogToInsert);

            await _uow.Connection.ExecuteAsync(storedProcedureName, parametters,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
        }

        /// <summary>
        /// Hàm thực hiện lấy nhật kí họat động đăng nhập, đăng xuất của người dùng thông qua id
        /// </summary>
        /// <param name="entity">Đối tượng cần tạo</param>
        /// <returns>DynamicParameters</returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<IEnumerable<LoginLog>> GetUserLoginLogsById(Guid userId)
        {
            string storedProcedureName = "Proc_loginlog_GetUserLoginLogById";

            var param = new DynamicParameters();
            param.Add("i_UserId", userId);

            var loginLogs = await _uow.Connection.QueryAsync<LoginLog>(storedProcedureName, param,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

            return loginLogs;
        }

        /// <summary>
        /// Hàm tạo param
        /// </summary>
        /// <param name="entity">Đối tượng cần tạo</param>
        /// <returns>DynamicParameters</returns>
        /// Author: PNNHai
        /// Date: 
        private DynamicParameters CreateParamettersFromEntity(LoginLog loginLogToInsert)
        {
            var parameters = new DynamicParameters();
            var properties = loginLogToInsert.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyName = $"i_{property.Name}";
                var propertyValue = property.GetValue(loginLogToInsert);
                parameters.Add(propertyName, propertyValue);
            }
            return parameters;
        }
        #endregion
    }
}
