using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class BaseAuditEntity
    {
        [NoMoreThanCurrentDate(ErrorMessage = BaseAuditAttributeValidationConst.CREATED_DATE_NO_MORE_THAN_CURRENT_DATE)]
        public DateTime? CreatedDate { set; get; }

        [MaxLength(255, ErrorMessage = BaseAuditAttributeValidationConst.CREATED_BY_NO_MORE_THAN_MAX_LENGTH)]
        public string? CreatedBy { set; get; }

        [NoMoreThanCurrentDate(ErrorMessage = BaseAuditAttributeValidationConst.MODIFIED_BY_NO_MORE_THAN_MAX_LENGTH)]
        public DateTime? ModifiedDate { set; get; }

        [MaxLength(255, ErrorMessage = BaseAuditAttributeValidationConst.MODIFIED_BY_NO_MORE_THAN_MAX_LENGTH)]
        public string? ModifiedBy { set; get; }
    }
}
