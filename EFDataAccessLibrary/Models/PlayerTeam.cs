using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class PlayerTeam
    {
        public int PlayerID { get; set; }
        public int TeamID { get; set; }

        public virtual Player Player { get; set; }
        public virtual Team Team { get; set; }

        public DateTime JoinDate { get; set; }
        public DateTime? ExitDate { get; set; }
    }
}