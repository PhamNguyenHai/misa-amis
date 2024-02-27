using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class UserAttributeValidationConst
    {
        // Login
        public const string USER_LOGIN_INFOR_REQUIRED = "Thông tin đăng nhập không được phép để trống !";
        public const string EMAIL_OR_PHONE_NUMBER_NO_MORE_THAN_MAX_LENGTH = "Email hoặc số điện thoại đăng nhập không được quá 100 kí tự !";
        public const string PASSWORD_NO_MORE_THAN_MAX_LENGTH = "Mật khẩu không được quá 50 kí tự !";

        // Register
        public const string FULL_NAME_REQUIRED = "Họ và tên không được phép để trống !";
        public const string PASSWORD_REQUIRED = "Mật khẩu không được phép để trống !";
        public const string EMAIL_REQUIRED = "Email không được phép để trống !";
        public const string PHONE_NUMBER_REQUIRED = "Số điện thoại không được phép để trống !";

        public const string FULL_NAME_NO_MORE_THAN_MAX_LENGTH = "Họ và tên không được phép quá 100 kí tự !";
        public const string EMAIL_NO_MORE_THAN_MAX_LENGTH = "Email không được phép quá 100 kí tự !";
        public const string PHONE_NUMBER_NO_MORE_THAN_MAX_LENGTH = "Số điện thoại không được phép quá 50 kí tự !";

        public const string USER_ID_REQUIRED = "Mã định danh tài khoản không được phép để trống !";
        public const string ROLE_REQUIRED = "Quyền tài khoản không được phép để trống !";
    }
}
