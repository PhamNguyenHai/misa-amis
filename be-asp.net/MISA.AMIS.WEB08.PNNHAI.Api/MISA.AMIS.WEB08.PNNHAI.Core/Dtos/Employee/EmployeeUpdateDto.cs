using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class EmployeeUpdateDto
    {
        [Required(ErrorMessage = EmployeeAttributeValidationConst.EMPLOYEE_CODE_REQUIRED)]
        [MaxLength(20, ErrorMessage = EmployeeAttributeValidationConst.EMPLOYEE_CODE_NO_MORE_THAN_MAX_LENGTH)]
        public string EmployeeCode { set; get; }

        [Required(ErrorMessage = EmployeeAttributeValidationConst.EMPLOYEE_NAME_REQUIRED)]
        [MaxLength(100, ErrorMessage = EmployeeAttributeValidationConst.EMPLOYEE_NAME_NO_MORE_THAN_MAX_LENGTH)]
        public string EmployeeName { set; get; }

        public bool? IsCustomer { set; get; }
        public bool? IsProvider { set; get; }


        [NoMoreThanCurrentDate(ErrorMessage = EmployeeAttributeValidationConst.DOB_NO_MORE_THAN_CURRENT_DATE)]
        public DateTime? DateOfBirth { set; get; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender Gender { set; get; }

        [Required(ErrorMessage = EmployeeAttributeValidationConst.DEPARTMENT_ID_REQUIRED)]
        public Guid DepartmentId { set; get; }


        [MaxLength(255, ErrorMessage = EmployeeAttributeValidationConst.POSITION_NAME_NO_MORE_THAN_MAX_LENGTH)]
        public string? PositionName { set; get; }


        [MaxLength(25, ErrorMessage = EmployeeAttributeValidationConst.IDENTITY_NUMBER_NO_MORE_THAN_MAX_LENGTH)]
        public string? IdentityNumber { set; get; }


        [NoMoreThanCurrentDate(ErrorMessage = EmployeeAttributeValidationConst.IDENTITY_ISSUED_DATE_NO_MORE_THAN_CURRENT_DATE)]
        public DateTime? IdentityIssuedDate { set; get; }


        [MaxLength(255, ErrorMessage = EmployeeAttributeValidationConst.IDENTITY_ISSUED_PLACE_NO_MORE_THAN_MAX_LENGTH)]
        public string? IdentityIssuedPlace { set; get; }


        [MaxLength(255, ErrorMessage = EmployeeAttributeValidationConst.ADDRESS_NO_MORE_THAN_MAX_LENGTH)]
        public string? Address { set; get; }


        [MaxLength(50, ErrorMessage = EmployeeAttributeValidationConst.PHONE_NUMBER_NO_MORE_THAN_MAX_LENGTH)]
        public string? PhoneNumber { set; get; }


        [MaxLength(50, ErrorMessage = EmployeeAttributeValidationConst.LANDLINE_NUMBER_NO_MORE_THAN_MAX_LENGTH)]
        public string? LandlineNumber { set; get; }


        [MaxLength(100, ErrorMessage = EmployeeAttributeValidationConst.EMAIL_NO_MORE_THAN_MAX_LENGTH)]
        [EmailAddress(ErrorMessage = EmployeeAttributeValidationConst.EMAIL_INVALID_FORMAT)]
        public string? Email { set; get; }


        [MaxLength(50, ErrorMessage = EmployeeAttributeValidationConst.BANK_NUMBER_NO_MORE_THAN_MAX_LENGTH)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = EmployeeAttributeValidationConst.BANK_NUMBER_INVALID_FORMAT)]
        public string? BankNumber { set; get; }


        [MaxLength(255, ErrorMessage = EmployeeAttributeValidationConst.BANK_NAME_NO_MORE_THAN_MAX_LENGTH)]
        public string? BankName { set; get; }


        [MaxLength(255, ErrorMessage = EmployeeAttributeValidationConst.BANK_BRANCH_NO_MORE_THAN_MAX_LENGTH)]
        public string? BankBranch { set; get; }
    }
}
