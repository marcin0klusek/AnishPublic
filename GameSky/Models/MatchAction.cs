using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class MatchAction
    {
        public MatchActionEnum Type { get; set; }
        public string Player1 { get; set; }
        public string? Player2 { get; set; }

    }
}
