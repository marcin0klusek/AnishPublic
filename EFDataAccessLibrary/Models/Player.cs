using EFDataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using RandomNameGeneratorLibrary;
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
        public PlayerCardQuality Quality { get; set; }
        public Boolean IsForSale { get; set; }
        public int PositionID { get; set; }
        public virtual PlayerPosition PlayerPosition { get; set; }
        public virtual ICollection<PlayerTeam> PlayerTeam { get; set; }


        static PersonNameGenerator generator = new();
        static Random random = new();

        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - this.BirthDate.Year;
            if (this.BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public int GetPrize()
        {
            return (int)Prize;
        }

        public static int GetSkillPrice(int skill)
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
            double qualiyFactor = 4/Math.Sqrt((int)Quality);
            this.PlayerLevel = (int)(1+(qualiyFactor * ((this.Aim * 3.33) + (this.Potencial * 3.33) + (this.Knowledge * 3.33))));
        }

        public void RecalcPrize()
        {
            double qualiyFactor = 10.0/((int)Quality);
            double ageFactor = (20.0 / GetAge()) * 20;
            double skillFactor = 2.68;

            this.Prize =(
                qualiyFactor *
                ((Math.Pow(skillFactor, this.Aim) + Math.Pow(skillFactor, this.Potencial) + Math.Pow(skillFactor, this.Knowledge))
                * ageFactor)
                );
        }

        public void PrintPlayersData()
        {
            Console.WriteLine("FirstName: " + FirstName);
            Console.WriteLine("LastName: " + LastName);
            Console.WriteLine("NickName: " + NickName);
            Console.WriteLine("BirthDate: " + BirthDate);
            Console.WriteLine("Aim: " + Aim);
            Console.WriteLine("Potencial: " + Potencial);
            Console.WriteLine("Knowledge: " + Knowledge);
            Console.WriteLine("Level: " + PlayerLevel);
            Console.WriteLine("Prize: " + Prize);
            Console.WriteLine("Quality: " + Quality);
        }
        public static Player GeneratePlayer(DataContext db)
        {
            Player p = new();
            p.FirstName = generator.GenerateRandomFirstName();
            p.LastName = generator.GenerateRandomLastName();
            p.NickName += p.FirstName.Length >= 3 ? p.FirstName[0..3] : p.FirstName;
            p.NickName += p.LastName.Length >= 3 ? p.LastName[0..3] : p.LastName;
            p.NickName += random.Next(100, 1000);

            //BirthDate generator
            DateTime start = new DateTime(1990, 1, 1);
            int range = ((DateTime.Today.AddYears(-10)) - start).Days;
            p.BirthDate =
             start.AddDays(random.Next(range));

            //Random pick of PlayerPosition
            var positions = db.GetPlayerPositions();
            p.PlayerPosition = positions.ElementAt(random.Next(0, positions.Count()));
            p.PositionID = p.PlayerPosition.PositionID;

            p.Potencial = random.Next(0, 4);
            p.Aim = random.Next(0, 4);
            p.Knowledge = random.Next(0, 4);
            p.Quality = GetRandomQuality();
            p.RecalcPlayerStats();
            return p;
        }

        private static PlayerCardQuality GetRandomQuality()
        {
            var qualities = Enum.GetValues(typeof(PlayerCardQuality)).Cast<PlayerCardQuality>().ToList();
            int total = 0;
            //Sum all values of Qualities
            foreach (var q in qualities)
            {
                total += (int)q;
            }

            var quality = random.Next(0, total + 1);
            //Pick the quality
            foreach (var q in qualities)
            {
                if (quality < (int)q)
                {
                    return q;
                }
                quality -= (int)q;
            }
            return PlayerCardQuality.iron;
        }
    }
}
