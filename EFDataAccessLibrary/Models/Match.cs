using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    [Table("Match")]
    public class Match
    {
        [Key]
        public int MatchID { get; set; }
        public virtual Team Team1 { get; set; }
        public virtual Team Team2 { get; set; }
        public int? ScoreTeam1 { get; set; }
        public int? ScoreTeam2 { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
#nullable enable
        public Map? Map { get; set; }
#nullable disable
        public Event Event { get; set; }

        public Boolean IsUpcoming()
        {
            if(StartDate > DateTime.Now)
            {
                return true;
            }
            return false;
        }

        public Boolean IsFinished()
        {
            if(ScoreTeam1 != null && ScoreTeam2 != null)
            {
                return true;
            }
            return false;
        }

        public Boolean IsPlaying()
        {
            if(!IsUpcoming() && EndDate is null)
            {
                return true;
            }
            return false;
        }
    }
}
