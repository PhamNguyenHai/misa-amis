using MISA.AMIS.WEB08.PNNHAI.Core.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class Department : BaseAuditEntity, IHasKey
    {
        [Required(ErrorMessage = DepartmentAttributeValidationConst.DEPARTMENT_ID_REQUIRED)]
        public Guid DepartmentId { get; set; }

        [Required(ErrorMessage = DepartmentAttributeValidationConst.DEPARTMENT_CODE_REQUIRED)]
        [MaxLength(20, ErrorMessage = DepartmentAttributeValidationConst.DEPARTMENT_CODE_NO_MORE_THAN_MAX_LENGTH)]
        public string DepartmentCode { get; set; }

        [Required(ErrorMessage = DepartmentAttributeValidationConst.DEPARTMENT_NAME_REQUIRED)]
        [MaxLength(255, ErrorMessage = DepartmentAttributeValidationConst.DEPARTMENT_NAME_NO_MORE_THAN_MAX_LENGTH)]
        public string DepartmentName { get; set; }

        public Guid GetKey()
        {
            return DepartmentId;
        }

        public void SetKey(Guid key)
        {
            DepartmentId = key;
        }
    }
}
