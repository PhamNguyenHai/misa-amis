using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface ITokenRepository
    {
        /// <summary>
        /// Hàm thực hiện lấy token thông qua refresh token
        /// </summary>
        /// <param name="refreshToken">refresh token cần lấy</param>
        /// <returns>Token ứng với phiên làm việc</returns>
        /// Author: PNNHai
        /// Date:
        Task<Token?> FindTokenByRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Hàm thực hiện thêm token vào db
        /// </summary>
        /// <param name="tokenToInsert">Token cần thêm mới</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task InsertTokenAsync(Token tokenToInsert);

        /// <summary>
        /// Thực hiện thu hồi token thông qua id
        /// </summary>
        /// <param name="tokenId">Token id muốn thu hồi</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task RevokeTokenByIdAsync(Guid tokenId);
    }
}
