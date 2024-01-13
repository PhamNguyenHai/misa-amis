using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IBaseRepository<TEntity, TModel> : IReadOnlyRepository<TEntity, TModel>
    {
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="entity">Thông tin sẽ sửa</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần xóa</param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Xóa nhiều đối tượng
        /// </summary>
        /// <param name="entities">Danh sách đối tượng cần xóa</param>
        /// <returns></returns>
        Task DeleteMultipalAsync(List<TEntity> entities);
    }
}
