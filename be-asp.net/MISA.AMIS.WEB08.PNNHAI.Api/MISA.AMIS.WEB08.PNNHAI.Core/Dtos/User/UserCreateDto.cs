using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = UserAttributeValidationConst.FULL_NAME_REQUIRED)]
        [MaxLength(100, ErrorMessage = UserAttributeValidationConst.FULL_NAME_NO_MORE_THAN_MAX_LENGTH)]
        public string FullName { get; set; }

        [Required(ErrorMessage = UserAttributeValidationConst.PASSWORD_REQUIRED)]
        [MaxLength(50, ErrorMessage = UserAttributeValidationConst.PASSWORD_NO_MORE_THAN_MAX_LENGTH)]
        public string Password { get; set; }

        [Required(ErrorMessage = UserAttributeValidationConst.EMAIL_REQUIRED)]
        [MaxLength(100, ErrorMessage = UserAttributeValidationConst.EMAIL_NO_MORE_THAN_MAX_LENGTH)]
        public string Email { get; set; }

        [Required(ErrorMessage = UserAttributeValidationConst.PHONE_NUMBER_REQUIRED)]
        [MaxLength(50, ErrorMessage = UserAttributeValidationConst.PHONE_NUMBER_NO_MORE_THAN_MAX_LENGTH)]
        public string PhoneNumber { get; set; }

        public UserRole Role { get; set; }
    }
}
