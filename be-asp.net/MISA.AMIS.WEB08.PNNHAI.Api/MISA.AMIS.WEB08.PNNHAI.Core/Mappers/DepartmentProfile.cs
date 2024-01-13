using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentModel, Department>();
            CreateMap<DepartmentModel, DepartmentDto>();
            CreateMap<FilterResult<DepartmentModel>, FilterResult<DepartmentDto>>();
        }
    }
}
