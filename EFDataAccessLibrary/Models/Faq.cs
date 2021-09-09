using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Faq
    {
        [Key]
        public int FaqID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModifyDate { get; set; }
        public string AuthorID { get; set; }
        public string LastModifierID { get; set; }
        public Boolean IsActive { get; set; }
        public virtual ICollection<FaqQuestion> FaqQuestions { get; set; }
    }
}
