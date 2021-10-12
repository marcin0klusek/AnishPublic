using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Change
    {
        [Key]
        public int ChangeId { get; set; }
        //Category title
        public string Title { get; set; }
        //Changes refers to category
        public virtual ICollection<ChangeElement> Elements { get; set; }
    }
}
