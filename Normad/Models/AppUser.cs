﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Normad.Models
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; }
    }
}
