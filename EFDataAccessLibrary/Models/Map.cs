using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    [Table("Map")]
    public class Map
    {
        [Key]
        public int MapID { get; set; }
        public String Name { get; set; }
    }
}
