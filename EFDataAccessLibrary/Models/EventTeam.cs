using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class EventTeam
    {
        public int TeamID { get; set; }
        public int EventID { get; set; }
        public virtual Team Team { get; set; }
        public virtual Event Event { get; set; }
    }
}
