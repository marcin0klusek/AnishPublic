using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class NewsContent
    {
        [Key]
        [ForeignKey("NewsHeader")]
        public int NewsContentID { get; set; }

        public string Content { get; set; }
    }
}