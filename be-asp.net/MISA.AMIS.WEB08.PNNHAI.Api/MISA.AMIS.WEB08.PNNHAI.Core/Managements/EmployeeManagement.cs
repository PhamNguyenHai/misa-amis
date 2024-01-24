using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core.Managements
{
    public class EmployeeManagement : IEmployeeManagement
    {
        #region Fields
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Constructor
        public EmployeeManagement(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion

        /// <summary>
        /// Hàm thực hiện kiểm tra mã trùng với nhân viên
        /// </summary>
        /// <param name="code">Mã nhân viên cần check</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">Mã đã tồn tại</exception>
        /// Author: PNNHai
        /// Date:
        public async Task CheckEmployeeExistByCode(string code)
        {
            var employeeExist = await _employeeRepository.FindByCodeAsync(code);

            if (employeeExist != null)
            {
                throw new ValidateException(string.Format(Core.Resources.AppResource.ExistedEmployeeCode, code));
            }
        }

        /// <summary>
        /// Hàm thực hiện kiểm tra mã nhân viên thực hiện update có thỏa mãn không
        /// Nếu mã khác so với mã id tìm đc -> kiểm tra xem mã tồn tại chưa
        /// Nếu chưa thì cho cập nhật | nếu tồn tại thì throw exception
        /// </summary>
        /// <param name="id">id nhân viên thực hiện</param>
        /// <param name="employeeCode">Mã thực hiện cập nhật</param>
        /// <returns></returns>
        public async Task CheckEmployeeCodeUpdateToExistedCode(Guid id, string employeeCode)
        {
            // Kiểm tra xem id truyền vào có tồn tại không (Nếu không throw exception ko tìm thấy)
            var employeeExist = await _employeeRepository.GetByIdAsync(id);

            // Nếu mã khác so với mã tìm từ id thì check xem mã đã tồn tại chưa 
            // Nếu mã tồn tại rồi thì exception cập nhật sang mã đã tồn tại
            if(employeeExist.EmployeeCode !=  employeeCode)
            {
                await CheckEmployeeExistByCode(employeeCode);
            }
        }
    }
}
