using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public abstract class BaseWithCodeService<TEntity, TModel, TEntityDto, TEntityCreateDto, TEntityUpdateDto>
        : BaseService<TEntity, TModel, TEntityDto, TEntityCreateDto, TEntityUpdateDto>,
        IBaseWithCodeService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto>
        where TEntity : IHasKey, IHasCode
    {
        #region Fields
        protected readonly IBaseWithCodeRepository<TEntity, TModel> _baseWithCodeRepository;
        #endregion

        #region Constructor
        public BaseWithCodeService(IBaseWithCodeRepository<TEntity, TModel> baseWithCodeRepository, IMapper mapper)
           : base(baseWithCodeRepository, mapper)
        {
            _baseWithCodeRepository = baseWithCodeRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Service lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<string> GetNewCodeAsync()
        {
            var newCode = await _baseWithCodeRepository.GetNewCodeAsync();
            return newCode;
        } 
        #endregion
    }
}
