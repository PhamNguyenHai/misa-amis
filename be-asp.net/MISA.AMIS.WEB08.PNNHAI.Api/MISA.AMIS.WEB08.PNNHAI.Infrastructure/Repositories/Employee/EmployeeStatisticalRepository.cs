using MISA.AMIS.WEB08.PNNHAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Infrastructure
{
    public class EmployeeStatisticalRepository : BaseStatisticalRepository, IEmployeeStatisticalRepository
    {
        #region Constructor
        public EmployeeStatisticalRepository(IUnitOfWork uow) : base(uow)
        {
            EntityName = "Employee";
        }
        #endregion

        public override string EntityName { get; protected set; }
    }
}
