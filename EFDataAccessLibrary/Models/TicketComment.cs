using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class TicketComment
    {
        public int TicketID { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int CommentID { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
