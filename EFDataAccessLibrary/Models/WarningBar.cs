using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class WarningBar
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public Boolean Active { get; set; }
        public DateTime PublishDate { get; set; }

        public bool IsActive()
        {
            return Active;
        }
    }
}
