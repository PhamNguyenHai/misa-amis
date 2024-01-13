using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.WEB08.PNNHAI.Core;

namespace MISA.AMIS.WEB08.PNNHAI.Api
{
    public class BaseWithCodeController<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto>
        : BaseController<TEntityDto, TEntityCreateDto, TEntityUpdateDto>
    {
        #region Fields
        private readonly IBaseWithCodeService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto> _baseWithCodeService;
        #endregion

        #region Constructor
        public BaseWithCodeController(IBaseWithCodeService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto> baseWithCodeService)
            : base(baseWithCodeService)
        {
            _baseWithCodeService = baseWithCodeService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Api lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// Author: PNNHai
        /// Date: 
        [HttpGet("NewCode")]
        public async Task<IActionResult> GetNewCode()
        {
            var result = await _baseWithCodeService.GetNewCodeAsync();
            return StatusCode(StatusCodes.Status200OK, result);
        } 
        #endregion
    }
}
