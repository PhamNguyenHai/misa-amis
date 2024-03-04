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

        public async Task ChangePasswordAsync(UserPasswordChangeDto userPasswordChange)
        {
            string storedProcedureName = "Proc_user_ChangePassword";

            var parametters = new DynamicParameters();
            parametters.Add("i_UserId", userPasswordChange.UserId);
            parametters.Add("i_ChangedPassword", userPasswordChange.ChangePassword);

            await _uow.Connection.ExecuteAsync(storedProcedureName, parametters,
                commandType: CommandType.StoredProcedure, transaction: _uow.Transaction);
        }

        /// <summary>
        /// Hàm thực hiện tìm kiếm người dùng thông qua email
        /// </summary>
        /// <param name="email">email của người dùng cần tìm</param>
        /// <returns>User</returns>
        /// Author: PNNHai
        /// Date:
        private async Task<User> FindUserByEmail(string email)
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
        /// <returns>User</returns>
        /// Author: PNNHai
        /// Date:
        private async Task<User> FindUserByPhoneNumber(string phoneNumber)
        {
            string sqlCommand = "SELECT * FROM view_user WHERE PhoneNumber = @phoneNumber";

            var parametters = new DynamicParameters();
            parametters.Add("@phoneNumber", phoneNumber);

            var existedUser = await _uow.Connection.QueryFirstOrDefaultAsync<User>(sqlCommand, parametters, transaction: _uow.Transaction);
            return existedUser;
        }

        public async Task CheckUserExistByEmail(string email)
        {
            var existedUser = await FindUserByEmail(email);
            if(existedUser != null)
            {
                throw new ValidateException("Email đã tồn tại trong hệ thống");
            }
        }

        public async Task CheckUserExistByPhoneNumber(string phoneNumber)
        {
            var existedUser = await FindUserByPhoneNumber(phoneNumber);
            if (existedUser != null)
            {
                throw new ValidateException("Số điện thoại đã tồn tại trong hệ thống");
            }
        }

        public async Task CheckUserEmailUpdateToExistedEmail(Guid id, string email)
        {
            // Kiểm tra xem id truyền vào có tồn tại không (Nếu không throw exception ko tìm thấy)
            var userExist = await GetByIdAsync(id);

            // Nếu cập nhật sang email khác -> kiểm tra xem email muốn cập nhật tồn tại chưa
            if (userExist.Email != email)
            {
                await CheckUserExistByEmail(email);
            }
        }

        public async Task CheckUserPhoneNumberUpdateToExistedPhoneNumber(Guid id, string phoneNumber)
        {
            // Kiểm tra xem id truyền vào có tồn tại không (Nếu không throw exception ko tìm thấy)
            var userExist = await GetByIdAsync(id);

            // Nếu cập nhật sang sdt khác -> kiểm tra xem sdt muốn cập nhật tồn tại chưa
            if (userExist.PhoneNumber != phoneNumber)
            {
                await CheckUserExistByPhoneNumber(phoneNumber);
            }
        }
        #endregion
    }
}
