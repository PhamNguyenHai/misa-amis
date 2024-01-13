using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public abstract class BaseService<TEntity, TModel, TEntityDto, TEntityCreateDto, TEntityUpdateDto>
        : ReadOnlyService<TEntity, TModel, TEntityDto>,
        IBaseService<TEntityDto, TEntityCreateDto, TEntityUpdateDto>
        where TEntity : IHasKey
    {
        #region Fields
        protected readonly IBaseRepository<TEntity, TModel> _baseRepository;
        #endregion

        #region Constructor
        protected BaseService(IBaseRepository<TEntity, TModel> baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
            _baseRepository = baseRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Service thực hiện thêm mới đối tượng
        /// </summary>
        /// <param name="entityCreateDto">Đối tượng cần thêm mới</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date: 
        public async Task CreateAsync(TEntityCreateDto entityCreateDto)
        {
            // Validate nghiệp vụ trước khi thêm mới
            await ValidateForInserting(entityCreateDto);

            var entity = _mapper.Map<TEntity>(entityCreateDto);
            entity.SetKey(Guid.NewGuid());
            if (entity is BaseAuditEntity entityAuditEntity)
            {
                entityAuditEntity.CreatedDate = DateTime.Now;
                entityAuditEntity.ModifiedDate = DateTime.Now;
                entityAuditEntity.CreatedBy = "admin";
                entityAuditEntity.ModifiedBy = "admin";
            }

            // insert vào DB
            await _baseRepository.InsertAsync(entity);

        }

        /// <summary>
        /// Service cập nhật đối tượng
        /// </summary>
        /// <param name="id">Id của đối tượng cần cập nhật</param>
        /// <param name="entityUpdateDto">Thông tin đối tượng cần cập nhật</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date: 
        public async Task UpdateAsync(Guid id, TEntityUpdateDto entityUpdateDto)
        {
            // Kiểm tra id truyền vào có chuẩn không. Nếu ko thì báo lỗi
            await _baseRepository.GetByIdAsync(id);

            // Sau khi id hợp lệ thì validate nghiệp vụ
            // Validate nghiệp vụ trước khi update
            await ValidateForUpdating(id, entityUpdateDto);

            var entity = _mapper.Map<TEntity>(entityUpdateDto);
            entity.SetKey(id);
            if (entity is BaseAuditEntity entityAuditEntity)
            {
                entityAuditEntity.ModifiedDate = DateTime.Now;
                entityAuditEntity.ModifiedBy = "admin";
            }

            // update vào DB
            await _baseRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// Service xóa đối tượng
        /// </summary>
        /// <param name="id">Id của đối tượng cần xóa</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date: 
        public async Task DeleteAsync(Guid id)
        {
            // Lấy entity theo id
            var model = await _baseRepository.GetByIdAsync(id);

            var entity = _mapper.Map<TEntity>(model);
            // Xóa 
            await _baseRepository.DeleteAsync(entity);
        }

        /// <summary>
        /// Service xóa nhiều đối tượng
        /// </summary>
        /// <param name="ids">Danh sách id của đối tượng cần xóa</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date: 
        public async Task DeleteMultipalAsync(List<Guid> ids)
        {
            // Validate kiểm tra xem có tồn tại Id nào không thỏa mãn không. Nếu có không cho xóa và throw excception
            // Sau đó chuyển list ids thành list entities
            var deleteEntities = await ValidateAndMapDeleteIdsToDeleteEntities(ids);

            await _baseRepository.DeleteMultipalAsync(deleteEntities);
        }

        /// <summary>
        /// Yêu cầu ghi đè lại ở class khởi tạo đối tượng
        /// Yêu cầu ghi đè lại tùy nghiệp vụ ở từng service
        /// </summary>
        /// <param name="entityCreateDto">Thông tin phần tử cần thêm</param>
        /// <returns></returns>
        /// author: PNNHai
        /// date: 
        public abstract Task ValidateForInserting(TEntityCreateDto entityCreateDto);

        /// <summary>
        /// Yêu cầu ghi đè lại ở class khởi tạo đối tượng
        /// Validate nghiệp vụ để update tùy service
        /// </summary>
        /// <param name="id">Mã định danh của phần tử cần update</param>
        /// <param name="entityUpdateDto">Thông tin update</param>
        /// <returns></returns>
        /// author: PNNHai
        /// date: 
        public abstract Task ValidateForUpdating(Guid id, TEntityUpdateDto entityUpdateDto);

        /// <summary>
        /// Hàm validate và chuyển ids -> entities -> xóa nhiều.
        /// Mặc định sẽ ktra xem ids truyền vào có thỏa mãn hết không. Nếu tồn tại Id không thỏa mãn sẽ throw exception
        /// Hàm này virtual cho phép ghi đè lại tùy thuộc vào nghiệp vụ của từng service
        /// </summary>
        /// <param name="ids">Danh sách mã định danh cần xóa</param>
        /// <returns>Danh sách phần tử cần xóa</returns>
        /// <exception cref="ValidateException">Tồn tại id không thỏa mãn</exception>
        public virtual async Task<List<TEntity>> ValidateAndMapDeleteIdsToDeleteEntities(List<Guid> ids)
        {
            // Validate
            if (ids.Count > 20)
            {
                throw new ValidateException(String.Format(Core.Resources.AppResource.MoreRecordDeleteError, 20));
            }

            // Thực hiện map và validate các id. Nếu tồn tại id không thỏa mãn thì hủy bỏ không thực hiện
            // và thông báo exception luôn
            var deleteEntities = new List<TEntity>();
            foreach (var id in ids)
            {
                var deleteModel = await _baseRepository.FindByIdAsync(id);
                if (deleteModel == null)
                {
                    throw new ValidateException(Core.Resources.AppResource.ContainsInvalidIdError);
                }
                else
                {
                    var deleteEntity = _mapper.Map<TEntity>(deleteModel);
                    deleteEntities.Add(deleteEntity);
                }
            }

            return deleteEntities;
        } 
        #endregion
    }
}
