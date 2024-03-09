﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class LoginLogDto
    {
        public Guid LoginId { get; set; }
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public string OperatingSystem { get; set; }
        public string DeviceName { get; set; }
        public DeviceType DeviceType { get; set; }
        public DateTime? LogoutDate { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedDate { set; get; }
    }
}