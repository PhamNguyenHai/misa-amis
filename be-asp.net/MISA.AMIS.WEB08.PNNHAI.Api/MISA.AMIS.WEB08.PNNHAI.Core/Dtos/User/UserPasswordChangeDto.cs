using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class UserPasswordChangeDto
    {
        [Required(ErrorMessage = UserAttributeValidationConst.PASSWORD_REQUIRED)]
        [MaxLength(50, ErrorMessage = UserAttributeValidationConst.PASSWORD_NO_MORE_THAN_MAX_LENGTH)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = UserAttributeValidationConst.PASSWORD_REQUIRED)]
        [MaxLength(50, ErrorMessage = UserAttributeValidationConst.PASSWORD_NO_MORE_THAN_MAX_LENGTH)]
        public string ChangePassword { get; set; }
    }
}
