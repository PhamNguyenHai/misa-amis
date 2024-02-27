using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class User : BaseAuditEntity, IHasKey
    {
        [Required(ErrorMessage = UserAttributeValidationConst.USER_ID_REQUIRED)]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = UserAttributeValidationConst.FULL_NAME_REQUIRED)]
        [MaxLength(100, ErrorMessage = UserAttributeValidationConst.FULL_NAME_NO_MORE_THAN_MAX_LENGTH)]
        public string FullName { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = UserAttributeValidationConst.EMAIL_REQUIRED)]
        [MaxLength(100, ErrorMessage = UserAttributeValidationConst.EMAIL_NO_MORE_THAN_MAX_LENGTH)]
        public string Email { get; set; }

        [Required(ErrorMessage = UserAttributeValidationConst.PHONE_NUMBER_REQUIRED)]
        [MaxLength(50, ErrorMessage = UserAttributeValidationConst.PHONE_NUMBER_NO_MORE_THAN_MAX_LENGTH)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = UserAttributeValidationConst.ROLE_REQUIRED)]
        public UserRole Role { get; set; }

        public Guid GetKey()
        {
            return UserId;
        }

        public void SetKey(Guid key)
        {
            UserId = key;
        }
    }
}
