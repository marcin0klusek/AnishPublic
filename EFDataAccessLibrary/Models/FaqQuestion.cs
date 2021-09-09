using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class FaqQuestion
    {
        public int FaqID { get; set; }
        public int QuestionID { get; set; }

        public virtual Faq Faq { get; set; }
        public virtual Question Question { get; set; }
    }
}
