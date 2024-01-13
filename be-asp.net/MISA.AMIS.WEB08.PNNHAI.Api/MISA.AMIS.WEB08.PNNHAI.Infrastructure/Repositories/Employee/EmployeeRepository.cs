using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public class EmployeeRepository : BaseWithCodeRepository<Employee, EmployeeModel>, IEmployeeRepository
    {
        #region Constructor
        public EmployeeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        } 
        #endregion
    }
}
