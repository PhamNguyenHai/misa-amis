using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IBaseWithCodeService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto>
        : IBaseService<TEntityDto, TEntityCreateDto, TEntityUpdateDto>
    {
        /// <summary>
        /// Service lấy ra mã mới
        /// </summary>
        /// <returns>mã mới</returns>
        /// Author: PNNHai
        /// Date: 
        Task<string> GetNewCodeAsync();
    }
}
