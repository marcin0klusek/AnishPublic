using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime LastModify { get; set; }
        [Required(ErrorMessage ="Podaj tytuł problemu")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Zgłoszenie musi posiadać opis problemu")]
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
    }
}
