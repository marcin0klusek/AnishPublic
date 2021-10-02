using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class TicketUser
    {
        public int TicketID { get; set; }
        public virtual Ticket Ticket { get; set; }
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
