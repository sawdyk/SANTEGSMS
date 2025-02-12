﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    public class SuperAdminInfoRespModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class SuperAdminForgotPasswordInfoRespModel : SuperAdminInfoRespModel
    {
        public string Password { get; set; }
    }
}
