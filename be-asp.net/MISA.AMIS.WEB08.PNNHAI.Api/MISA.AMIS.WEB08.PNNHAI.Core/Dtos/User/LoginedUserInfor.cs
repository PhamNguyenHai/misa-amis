using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class LoginedUserInfor
    {
        public string AccessToken { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public UserRole UserRole { get; set; }
    }
}
