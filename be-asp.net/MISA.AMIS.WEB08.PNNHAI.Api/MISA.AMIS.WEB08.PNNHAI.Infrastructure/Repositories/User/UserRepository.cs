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
        /// Hàm thực hiện lấy thông tin user thông qua thông tin đăng nhập người dùng 
        /// </summary>
        /// <param name="emailOrPhoneNumber">email hoặc số điện thoại đăng nhập</param>
        /// <param name="password">mật khẩu</param>
        /// <returns>Người dùng thỏa mãn</returns>
        /// Author: PNNHai
        /// Date:
        public async Task<User?> FindUserByLoginInforAsync(string emailOrPhoneNumber, string password)
        {
            string storedProcedureName = "Proc_user_CheckLoginInfor";

            var parametters = new DynamicParameters();
            parametters.Add("i_EmailOrPhoneNumber", emailOrPhoneNumber);
            parametters.Add("i_Password", password);

            var user = await _uow.Connection.QueryFirstOrDefaultAsync<User>(storedProcedureName, parametters,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
            return user;
        }

        public async Task ChangePasswordAsync(Guid id, string newPassword)
        {
            string storedProcedureName = "Proc_user_ChangePassword";

            var parametters = new DynamicParameters();
            parametters.Add("i_UserId", id);
            parametters.Add("i_ChangedPassword", newPassword);

            await _uow.Connection.ExecuteAsync(storedProcedureName, parametters,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
        }

        /// <summary>
        /// Hàm thực hiện tìm kiếm người dùng thông qua email
        /// </summary>
        /// <param name="email">email của người dùng cần tìm</param>
        /// <returns>User || null</returns>
        /// Author: PNNHai
        /// Date:
        public async Task<User?> FindUserByEmail(string email)
        {
            string sqlCommand = "SELECT * FROM view_user WHERE Email = @email";

            var parametters = new DynamicParameters();
            parametters.Add("@email", email);

            var existedUser = await _uow.Connection.QueryFirstOrDefaultAsync<User>(sqlCommand, parametters, transaction: _uow.Transaction);
            return existedUser;
        }

        /// <summary>
        /// Hàm thực hiện tìm kiếm người dùng thông qua sdt
        /// </summary>
        /// <param name="phoneNumber">sđt của người dùng cần tìm</param>
        /// <returns>User || null</returns>
        /// Author: PNNHai
        /// Date:
        public async Task<User?> FindUserByPhoneNumber(string phoneNumber)
        {
            string sqlCommand = "SELECT * FROM view_user WHERE PhoneNumber = @phoneNumber";

            var parametters = new DynamicParameters();
            parametters.Add("@phoneNumber", phoneNumber);

            var existedUser = await _uow.Connection.QueryFirstOrDefaultAsync<User>(sqlCommand, parametters, transaction: _uow.Transaction);
            return existedUser;
        }

        public async Task<bool> IsPasswordMatched(Guid id, string password)
        {
            string functionExecuteCommand = $"SELECT Func_user_CheckPasswordExisted(@id, @password)";

            var parameters = new DynamicParameters();
            parameters.Add($"@id", id);
            parameters.Add($"@password", password);

            // Kiểm tra khớp mật khẩu không
            var isMatched = await _uow.Connection.QueryFirstOrDefaultAsync<bool>(functionExecuteCommand, parameters, 
                transaction: _uow.Transaction);
            return isMatched;
        }
        #endregion
    }
}
