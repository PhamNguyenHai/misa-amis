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
    public class UserRepository : BaseRepository<User, UserModel>, IUserRepository
    {
        #region Constructor
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hàm thực hiện kiểm tra thông tin đăng nhập người dùng 
        /// </summary>
        /// <param name="emailOrPhoneNumber">email hoặc số điện thoại đăng nhập</param>
        /// <param name="password">mật khẩu</param>
        /// <returns>Người dùng thỏa mãn</returns>
        /// Author: PNNHai
        /// Date:
        public async Task<User?> CheckLoginInforAsync(string emailOrPhoneNumber, string password)
        {
            string storedProcedureName = "Proc_user_CheckLoginInfor";

            var parametters = new DynamicParameters();
            parametters.Add("i_EmailOrPhoneNumber", emailOrPhoneNumber);
            parametters.Add("i_Password", password);

            var user = await _uow.Connection.QueryFirstOrDefaultAsync<User>(storedProcedureName, parametters,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return user;
        }

        public Task RevokeTokenAsync(Guid tokenId)
        {
            throw new NotImplementedException();
        }

        public Task ChangePasswordAsync(UserPasswordChangeDto userPasswordChange)
        {
            throw new NotImplementedException();
        }

        //public Task RegisterAccountAsync(UserRegisterDto registerInfor, UserRole role)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<User?> FindByIdAsync(Guid userId)
        //{
        //    string storedProcedureName = "Proc_user_GetById";

        //    var param = new DynamicParameters();
        //    param.Add("i_UserId", userId);

        //    var user = await _uow.Connection.QueryFirstOrDefaultAsync<User>(storedProcedureName, param,
        //        commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);

        //    return user;
        //}
        #endregion
    }
}
