using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class ChangeElement
    {
        [Key]
        public int ChangeElementID { get; set; }
        public int ChangeID { get; set; }
        public string Text { get; set; }
    }
}
