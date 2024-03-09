using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class EmployeeService
        : BaseWithCodeService<Employee, EmployeeModel, EmployeeDto, EmployeeCreateDto, EmployeeUpdateDto>, IEmployeeService
    {
        #region Fields
        protected readonly IEmployeeRepository _employeeRepository;
        protected readonly IDepartmentRepository _departmentRepository;
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, IMapper mapper)
            : base(employeeRepository, mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }
        #endregion

        #region Methods
        public override async Task ValidateForInserting(EmployeeCreateDto entityCreateDto)
        {
            //                  Validate nghiệp vụ
            // Check mã nhân viên chưa tồn tại ? -> nếu đã tồn tại -> exception
            await CheckEmployeeCodeNotExistedByCode(entityCreateDto.EmployeeCode);

            // Check xem phòng ban có tồn tại không -> nếu ko -> exception
            await CheckDepartmentExisted(entityCreateDto.DepartmentId);
        }

        public override async Task ValidateForUpdating(Guid id, EmployeeUpdateDto entityUpdateDto)
        {
            //                  Validate nghiệp vụ
            // Kiểm tra xem id truyền vào có tồn tại không (Nếu không throw exception ko tìm thấy)
            var employeeExist = await GetByIdAsync(id);

            // Kiểm tra mã nhân viên muốn cập nhật có thay đổi so với ban đầu không. Nếu có kiểm tra
            // xem nó có được cập nhật sang mã chưa tồn tại không (nếu không throw exception)
            if (employeeExist.EmployeeCode != entityUpdateDto.EmployeeCode)
            {
                await CheckEmployeeCodeNotExistedByCode(entityUpdateDto.EmployeeCode);
            }

            // Check xem phòng ban có tồn tại không -> nếu ko -> exception
            await CheckDepartmentExisted(entityUpdateDto.DepartmentId);
        }

        /// <summary>
        /// Hàm thực hiện kiểm tra xem mã nhân viên chưa tồn tại trong hệ thống không
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên cần check xem tồn tại chưa</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">Mã nhân viên đã tồn tại</exception>
        /// Author: PNNHai
        /// Date
        private async Task CheckEmployeeCodeNotExistedByCode(string employeeCode)
        {
            // Kiểm tra mã nhân viên có tồn tại chưa (nếu chưa thì ok nếu rồi thì throw exception)
            var employeeExist = await _employeeRepository.FindByCodeAsync(employeeCode);

            if (employeeExist != null)
            {
                throw new ValidateException(string.Format(Core.Resources.AppResource.ExistedEmployeeCode, employeeCode));
            }
        }

        /// <summary>
        /// Hàm thực hiện kiểm tra xem phòng ban có tồn tại không
        /// </summary>
        /// <param name="departmentId">Mã định danh của phòng ban cần check</param>
        /// <exception cref="ValidateException">Phòng ban không tồn tại</exception>
        /// Author: PNNHai
        /// Date
        private async Task CheckDepartmentExisted(Guid departmentId)
        {
            var departmentExisted = await _departmentRepository.FindByIdAsync(departmentId);
            if(departmentExisted == null)
            {
                throw new ValidateException(Core.Resources.AppResource.DepartmentNotExistedError);
            }
        }
        #endregion
    }
}
