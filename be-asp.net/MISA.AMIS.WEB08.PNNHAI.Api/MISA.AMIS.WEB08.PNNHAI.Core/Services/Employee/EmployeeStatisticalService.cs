﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class EmployeeStatisticalService : BaseStatisticalService, IEmployeeStatisticalService
    {
        public EmployeeStatisticalService(IEmployeeStatisticalRepository employeeStatisticalRepository) : base(employeeStatisticalRepository)
        {
        }
    }
}
