using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class ApplicationUser : IdentityUser
    {
        public double Gold { get; set; }
        public int Dollars { get; set; }
    }
}
