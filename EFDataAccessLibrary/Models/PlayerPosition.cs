using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    [Table("PlayerPosition")]
    public class PlayerPosition
    {
        [Key]
        [ForeignKey("Player")]
        public int PositionID { get; set; }
        [StringLength(8)]
        public string Name { get; set; }
    }

}
