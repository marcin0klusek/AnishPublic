using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class MarketPlayer
    {
        public ApplicationUser User { get; set; }
        public Player Player { get; set; }
        public string UserID { get; set; }
        public int PlayerID { get; set; }
        public DateTime ExpireDate { get; set; }
        public Boolean IsBought { get; set; }
        public Boolean IsHidden { get; set; }
    }
}
