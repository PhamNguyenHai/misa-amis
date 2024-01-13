using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.WEB08.PNNHAI.Core;

namespace MISA.AMIS.WEB08.PNNHAI.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ReadOnlyController<DepartmentDto>
    {
        #region Fields
        private readonly IDepartmentService _departmentService;
        #endregion

        #region Constructor
        public DepartmentsController(IDepartmentService departmentService)
            : base(departmentService)
        {
            _departmentService = departmentService;
        }
        #endregion

        #region Methods
        public async override Task<IActionResult> FilterPaging(FilterInput filterInput)
        {
            return Ok("Tính năng chưa được khai thác");
        } 
        #endregion
    }
}
