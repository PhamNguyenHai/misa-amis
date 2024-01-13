using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.WEB08.PNNHAI.Core;

namespace MISA.AMIS.WEB08.PNNHAI.Api
{
    public class ReadOnlyController<TEntityDto> : ControllerBase
    {
        #region Fields
        private readonly IReadOnlyService<TEntityDto> _readOnlyService;
        #endregion

        #region Constructor
        public ReadOnlyController(IReadOnlyService<TEntityDto> readOnlyService)
        {
            _readOnlyService = readOnlyService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Api lấy danh sách tất cả đối tượng
        /// </summary>
        /// <returns>Danh sách đối tượng</returns>
        /// Author: PNNHai
        /// Date: 
        [HttpGet]
        public virtual async Task<IEnumerable<TEntityDto>> GetAll()
        {
            var result = await _readOnlyService.GetAllAsync();
            return result;
        }

        /// <summary>
        /// Api lấy đối tượng theo id
        /// </summary>
        /// <param name="id">Mã định danh đối tượng</param>
        /// <returns>Đối tượng cần lấy</returns>
        /// Author: PNNHai
        /// Date: 
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(Guid id)
        {
            var result = await _readOnlyService.GetByIdAsync(id);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Api lọc dữ liệu kết hợp phân trang, tìm kiếm và sắp xếp
        /// </summary>
        /// <param name="filterInput">Dữ liệu lọc ....</param>
        /// <returns>Kết quả phân trang</returns>
        /// Author: PNNHai
        /// Date: 
        [HttpPost("FilterPaging")]
        public virtual async Task<IActionResult> FilterPaging(FilterInput filterInput)
        {
            var result = await _readOnlyService.FilterPagingAsync(filterInput);
            return StatusCode(StatusCodes.Status200OK, result);
        } 
        #endregion
    }
}
