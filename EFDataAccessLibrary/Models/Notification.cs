using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime CreateAt { get; set; }
        public Boolean Readed { get; set; }
    }
}
