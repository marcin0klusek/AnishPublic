using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class NewsUpdate
    {
        [Key]
        [ForeignKey("NewsHeader")]
        public int UpdateId { get; set; }
        public virtual ICollection<Change> Changes { get; set; }
    }
}
