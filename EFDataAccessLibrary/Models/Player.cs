using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccessLibrary.Models
{
    [Table("Player")]
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
        public double Prize { get; set; }
        public int Potencial { get; set; }
        public int Aim { get; set; }
        public int Knowledge { get; set; }
        public int PlayerLevel { get; set; }
        public int PositionID { get; set; }
        public virtual PlayerPosition PlayerPosition { get; set; }
        public virtual ICollection<PlayerTeam> PlayerTeam { get; set; }

        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - this.BirthDate.Year;
            if (this.BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public static int GetSkillsPrice(int skill)
        {
            switch (skill)
            {
                case 0:
                    return 500;
                case 1:
                case 2:
                case 3:
                    return 1500;
                case 4:
                case 5:
                    return 3000;
                case 6:
                case 7:
                    return 5000;
                case 8:
                    return 6500;
                case 9:
                    return 8000;
                case 10:
                    return -1;
            }
            return -10;
        }

        public void RecalcPlayerStats()
        {
            RecalcLevel();
            RecalcPrize();
        }

        public void RecalcLevel()
        {
            this.PlayerLevel = ((int)((this.Aim + this.Potencial + this.Knowledge) * 3.33))+1;
        }

        public void RecalcPrize()
        {
            this.Prize = (int)((Math.Pow(2, this.Aim) * 10) + (Math.Pow(4, this.Knowledge) * 10) + (Math.Pow(3, this.Potencial) * Math.Sqrt(7)));
        }
    }
}
