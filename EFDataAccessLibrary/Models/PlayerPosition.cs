using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class PlayerPosition
    {
        [Key]
        [ForeignKey("Player")]
        public int PositionID { get; set; }
        [StringLength(8)]
        public string Name { get; set; }
        public override String ToString()
        {
            if(Name != null)
                return Name;
            return "";
        }
    }

}
