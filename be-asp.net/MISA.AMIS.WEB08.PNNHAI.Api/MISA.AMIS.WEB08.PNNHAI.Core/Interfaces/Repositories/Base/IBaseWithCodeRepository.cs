using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IBaseWithCodeRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel>
    {
        /// <summary>
        /// Tìm kiếm phần tử theo code (Nếu ko có -> null)
        /// </summary>
        /// <param name="code">Mã của phần tử</param>
        /// <returns>Phần tử tương ứng</returns>
        /// Author: PNNHai
        /// Date: 
        Task<TEntity?> FindByCodeAsync(string code);

        /// <summary>
        /// Trả về mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// Author: PNNHai
        /// Date:
        Task<string> GetNewCodeAsync();
    }
}
