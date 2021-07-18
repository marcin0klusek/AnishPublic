using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    public class Player
    {
        [Key]
        public int PlayerID { get; set; }
        [StringLength(64)]
        public string FirstName { get; set; }
        [StringLength(64)]
        public string LastName { get; set; }
        [StringLength(64)]
        public string NickName { get; set; }
        public DateTime BirthDate { get; set; }
        public float Prize { get; set; }
        public int Potencial { get; set; }
        public int Aim { get; set; }
        public int Knowledge { get; set; }
        public int PlayerLevel { get; set; }
        public virtual PlayerPosition PlayerPosition { get; set; }


    }
}
