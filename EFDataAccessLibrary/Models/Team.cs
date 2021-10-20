using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [StringLength(2)]
        public string Country { get; set; }
#nullable enable
        public string? OwnerID { get; set; }
#nullable disable
        public virtual ICollection<PlayerTeam> PlayerTeam { get; set; }
        public virtual ICollection<EventTeam> EventTeams { get; set; }
        [NotMapped]
        public List<Player> Players { get; set; }

        public List<Player> GetActiveRoster()
        {
            return PlayerTeam
                .Where(x => x.IsInActiveRoster)
                .Select(x => x.Player)
                .OrderByDescending(x => x.Quality).ThenByDescending(x => x.PlayerLevel)
                .ToList();
        }

        public List<Player> GetAdditionalPlayers()
        {
            return PlayerTeam
                .Where(x => !x.IsInActiveRoster)
                .Select(x => x.Player)
                .OrderBy(x => x.Quality).ThenByDescending(x => x.PlayerLevel)
                .ToList();
        }

        public List<Player> GetPastPlayers()
        {
            return PlayerTeam
                .Where(pt => pt.ExitDate != null)
                .OrderByDescending(ex => ex.ExitDate)
                .Select(p => p.Player)
                .ToList();
        }
    }
}
