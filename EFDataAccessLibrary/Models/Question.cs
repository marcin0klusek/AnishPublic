using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }
        public virtual ICollection<FaqQuestion> FaqQuestions { get; set; }

    }
}
