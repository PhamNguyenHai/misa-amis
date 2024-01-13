using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public static class EmployeeAttributeValidationConst
    {
        // required
        public const string EMPLOYEE_ID_REQUIRED = "Mã định danh nhân viên không được để trống !";
        public const string EMPLOYEE_CODE_REQUIRED = "Mã nhân viên không được để trống !";
        public const string EMPLOYEE_NAME_REQUIRED = "Tên nhân viên không được để trống !";
        public const string DEPARTMENT_ID_REQUIRED = "Phòng ban không được để trống !";

        // no more than current date
        public const string DOB_NO_MORE_THAN_CURRENT_DATE = "Ngày sinh không được vượt quá ngày hiện tại !";
        public const string IDENTITY_ISSUED_DATE_NO_MORE_THAN_CURRENT_DATE = "Ngày cấp CCCD không được vượt quá ngày hiện tại !";

        // invalid format
        public const string EMAIL_INVALID_FORMAT = "Email sai định dạng !";
        public const string BANK_NUMBER_INVALID_FORMAT = "Số tài khoản ngân hàng sai định dạng !";

        // invalid length
        public const string EMPLOYEE_CODE_NO_MORE_THAN_MAX_LENGTH = "Mã nhân viên tối đa 20 kí tự !";
        public const string EMPLOYEE_NAME_NO_MORE_THAN_MAX_LENGTH = "Tên nhân viên tối đa 100 kí tự !";
        public const string POSITION_NAME_NO_MORE_THAN_MAX_LENGTH = "Tên chức danh tối đa 255 kí tự !";
        public const string IDENTITY_NUMBER_NO_MORE_THAN_MAX_LENGTH = "Căn cước công dân tối đa 25 kí tự !";
        public const string IDENTITY_ISSUED_PLACE_NO_MORE_THAN_MAX_LENGTH = "Nơi cấp CCCD tối đa 255 kí tự !";
        public const string ADDRESS_NO_MORE_THAN_MAX_LENGTH = "Địa chỉ tối đa 255 kí tự !";

        public const string PHONE_NUMBER_NO_MORE_THAN_MAX_LENGTH = "Số di động tối đa 50 kí tự !";
        public const string LANDLINE_NUMBER_NO_MORE_THAN_MAX_LENGTH = "Số điện thoại cố định tối đa 50 kí tự !";
        public const string EMAIL_NO_MORE_THAN_MAX_LENGTH = "Email tối đa 100 kí tự !";
        public const string BANK_NUMBER_NO_MORE_THAN_MAX_LENGTH = "Số tài khoản ngân hàng tối đa 50 kí tự !";
        public const string BANK_NAME_NO_MORE_THAN_MAX_LENGTH = "Tên ngân hàng tối đa 255 kí tự !";
        public const string BANK_BRANCH_NO_MORE_THAN_MAX_LENGTH = "Chi nhánh ngân hàng tối đa 255 kí tự !";

    }
}
