using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IReadOnlyService<TEntityDto>
    {
        /// <summary>
        /// Service lấy toàn bộ đối tượng
        /// </summary>
        /// <returns>Danh sách đối tượng</returns>
        /// Author: PNNHai
        /// Date: 
        Task<IEnumerable<TEntityDto>> GetAllAsync();

        /// <summary>
        /// Service lấy thông tin đối tượng theo id
        /// </summary>
        /// <param name="id">Mã định danh của đối tượng</param>
        /// <returns>Đối tượng cần lấy</returns>
        /// Author: PNNHai
        /// Date: 
        Task<TEntityDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Service lọc dữ liệu kết hợp phân trang, sắp xếp và tìm kiếm
        /// </summary>
        /// <param name="filterInput">Đầu vào để lọc, phân trang, sắp xếp và tìm kiếm</param>
        /// <returns>Danh sách đối tượng kết hợp phân trang</returns>
        /// Author: PNNHai
        /// Date: 
        Task<FilterResult<TEntityDto>> FilterPagingAsync(FilterInput filterInput);
    }
}
