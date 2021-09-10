using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public double Gold { get; set; }
        public int Dollars { get; set; }
        public int? OwningTeamId { get; set; }
        public virtual Team OwningTeam { get; set; }
    }
}
