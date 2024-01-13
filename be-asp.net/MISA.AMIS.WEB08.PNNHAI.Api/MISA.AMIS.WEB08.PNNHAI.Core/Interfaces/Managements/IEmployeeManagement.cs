using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public interface IEmployeeManagement
    {
        /// <summary>
        /// Validate mã trùng
        /// </summary>
        /// <param name="code">Code muốn check</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task CheckEmployeeExistByCode(string code);

        /// <summary>
        /// Validate mã để cập nhật có bị thay đổi không.Nếu có thì có update sang mã chưa tồn tại không
        /// </summary>
        /// <param name="id">Mã định danh cần check</param>
        /// <param name="newEmployeeCode">Mã cần check</param>
        /// <returns></returns>
        /// Author: PNNHai
        /// Date:
        Task CheckEmployeeCodeUpdateToExistedCode(Guid id, string newEmployeeCode);
    }
}