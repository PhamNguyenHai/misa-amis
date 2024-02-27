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
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
            : base(employeeRepository, mapper)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Methods
        public override async Task ValidateForInserting(EmployeeCreateDto entityCreateDto)
        {
            //                  Validate nghiệp vụ
            // Kiểm tra mã nhân viên có tồn tại chưa (nếu chưa thì ok nếu rồi thì throw exception)
            await _employeeRepository.CheckEmployeeExistByCode(entityCreateDto.EmployeeCode);
        }

        public override async Task ValidateForUpdating(Guid id, EmployeeUpdateDto entityUpdateDto)
        {
            //                  Validate nghiệp vụ
            // Kiểm tra mã nhân viên muốn cập nhật có thay đổi so với ban đầu không. Nếu có kiểm tra
            // xem nó có được cập nhật sang mã chưa tồn tại không (nếu không throw exception)
            await _employeeRepository.CheckEmployeeCodeUpdateToExistedCode(id, entityUpdateDto.EmployeeCode);
        }
        #endregion
    }
}
