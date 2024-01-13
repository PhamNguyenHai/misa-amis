using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IBaseService<TEntityDto, TEntityCreateDto, TEntityUpdateDto>
        : IReadOnlyService<TEntityDto>
    {
        /// <summary>
        /// Service thêm mới đối tượng
        /// </summary>
        /// <param name="entityCreateDto">Phần tử cần thêm mới</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date: 
        Task CreateAsync(TEntityCreateDto entityCreateDto);

        /// <summary>
        /// Service cập nhật đối tượng
        /// </summary>
        /// <param name="id">Mã định danh cần cập nhật</param>
        /// <param name="entityUpdateDto">Thông tin đối tượng update</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date: 
        Task UpdateAsync(Guid id, TEntityUpdateDto entityUpdateDto);

        /// <summary>
        /// Service xóa đối tượng
        /// </summary>
        /// <param name="id">Mã định danh của đối tượng cần xóa</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date: 
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Service xóa nhiều đối tượng
        /// </summary>
        /// <param name="ids">Danh sách mã định danh cần xóa</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date: 
        Task DeleteMultipalAsync(List<Guid> ids);

    }
}
