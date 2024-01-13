using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public abstract class ReadOnlyService<TEntity, TModel, TEntityDto> : IReadOnlyService<TEntityDto> where TEntity : IHasKey
    {
        #region Fields
        protected readonly IReadOnlyRepository<TEntity, TModel> _readOnlyRepository;
        protected readonly IMapper _mapper;
        #endregion

        #region Constructor
        protected ReadOnlyService(IReadOnlyRepository<TEntity, TModel> readOnlyRepository, IMapper mapper)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Service lấy toàn bộ danh sách đối tượng
        /// </summary>
        /// <returns>Danh sách đối tượng </returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<IEnumerable<TEntityDto>> GetAllAsync()
        {
            var entities = await _readOnlyRepository.GetAllAsync();
            var entityDtos = _mapper.Map<IEnumerable<TEntityDto>>(entities);
            return entityDtos;
        }

        /// <summary>
        /// Service lấy đối tượng theo id
        /// </summary>
        /// <param name="id">Mã định danh đối tượng</param>
        /// <returns>Đối tượng cần lấy</returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<TEntityDto> GetByIdAsync(Guid id)
        {
            var entity = await _readOnlyRepository.GetByIdAsync(id);
            var entityDto = _mapper.Map<TEntityDto>(entity);
            return entityDto;
        }

        /// <summary>
        /// Service lọc dữ liệu kết hợp phân trang, tìm kiếm và sắp xếp
        /// </summary>
        /// <param name="filterInput">Dữ liệu lọc ....</param>
        /// <returns>Kết quả phân trang</returns>
        /// Author: PNNHai
        /// Date: 
        public async Task<FilterResult<TEntityDto>> FilterPagingAsync(FilterInput filterInput)
        {
            var entity = await _readOnlyRepository.FilterPagingAsync(filterInput);
            var entityDto = _mapper.Map<FilterResult<TEntityDto>>(entity);
            return entityDto;
        } 
        #endregion
    }
}
