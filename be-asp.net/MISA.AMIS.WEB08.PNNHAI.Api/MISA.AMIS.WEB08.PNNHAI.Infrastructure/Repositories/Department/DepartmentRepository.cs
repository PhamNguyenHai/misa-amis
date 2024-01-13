using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public class DepartmentRepository : ReadOnlyRepository<Department, DepartmentModel>, IDepartmentRepository
    {
        #region Constructor
        public DepartmentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        } 
        #endregion
    }
}
