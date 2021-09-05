using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public string Name { get; set; }
        public double PrizePool { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
        public virtual ICollection<EventTeam> EventTeams { get; set; }
    }
}
