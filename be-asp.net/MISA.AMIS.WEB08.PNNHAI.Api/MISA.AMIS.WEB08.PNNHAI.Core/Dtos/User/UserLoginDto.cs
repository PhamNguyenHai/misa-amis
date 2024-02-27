using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = UserAttributeValidationConst.USER_LOGIN_INFOR_REQUIRED)]
        [MaxLength(100, ErrorMessage = UserAttributeValidationConst.EMAIL_OR_PHONE_NUMBER_NO_MORE_THAN_MAX_LENGTH)]
        public string EmailOrPhoneNumber { get; set; }

        [Required(ErrorMessage = UserAttributeValidationConst.USER_LOGIN_INFOR_REQUIRED)]
        [MaxLength(50, ErrorMessage = UserAttributeValidationConst.PASSWORD_NO_MORE_THAN_MAX_LENGTH)]
        public string Password { get; set; }
    }
}
