using Dapper;
using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public class TokenRepository : ITokenRepository
    {
        #region Fields
        protected readonly IUnitOfWork _uow;
        #endregion

        #region Constructor
        public TokenRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }

        #endregion

        #region Methods

        public async Task<Token?> FindTokenByRefreshTokenAsync(string refreshToken)
        {
            string storedProcedureName = "Proc_token_GetByRefreshToken";

            var param = new DynamicParameters();
            param.Add("i_RefreshToken", refreshToken);

            var token = await _uow.Connection.QueryFirstOrDefaultAsync<Token>(storedProcedureName, param,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return token;
        }

        public async Task InsertTokenAsync(Token tokenToInsert)
        {
            string storedProcedureName = "Proc_token_Insert";

            // Chuyển entity sang parametters để truyền vào procedure
            var parametters = CreateParamettersFromEntity(tokenToInsert);

            await _uow.Connection.ExecuteAsync(storedProcedureName, parametters,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
        }

        public async Task RevokeTokenByIdAsync(Guid tokenId)
        {
            string storedProcedureName = "Proc_token_RevokeToken";

            // Param
            var param = new DynamicParameters();
            param.Add("i_TokenId", tokenId);

            await _uow.Connection.ExecuteAsync(storedProcedureName, param,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
        }

        /// <summary>
        /// Hàm tạo param
        /// </summary>
        /// <param name="entity">Đối tượng cần tạo</param>
        /// <returns>DynamicParameters</returns>
        /// Author: PNNHai
        /// Date: 
        private DynamicParameters CreateParamettersFromEntity(Token tokenToInsert)
        {
            var parameters = new DynamicParameters();
            var properties = tokenToInsert.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyName = $"i_{property.Name}";
                var propertyValue = property.GetValue(tokenToInsert);
                parameters.Add(propertyName, propertyValue);
            }
            return parameters;
        }
        #endregion
    }
}
