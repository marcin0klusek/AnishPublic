using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int TicketID { get; set; }
    }
}
