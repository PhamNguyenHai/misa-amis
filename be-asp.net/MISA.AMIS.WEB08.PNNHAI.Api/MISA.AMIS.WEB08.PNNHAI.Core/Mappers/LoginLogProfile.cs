﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.WEB08.PNNHAI.Core
{
    public class LoginLogProfile : Profile
    {
        public LoginLogProfile()
        {
            CreateMap<LoginLog, LoginLogDto>();
        }
    }
}
