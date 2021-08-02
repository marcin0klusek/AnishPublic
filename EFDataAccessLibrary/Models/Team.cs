using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Team
    {
        [Key]
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        [StringLength(8)]
        public string Tag { get; set; }
        public virtual ICollection<PlayerTeam> PlayerTeam { get; set; }
    }
}
