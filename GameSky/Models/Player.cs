using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class Player
    {
        public static List<Player> playersG2 = new List<Player>();
        public static List<Player> playersNavi = new List<Player>();
        public static int navi = 0;
        public static int g2 = 0;


        public string nickname;
        public string country = Country.GetCountry();
        public string photo;

        public Player(string team)
        {
            if(team.Equals("g2") || team.Equals("navi"))
                this.SetPhoto(team);

            if (team.Equals("g2"))
            {
                playersG2.Add(this);
            }
            else if(team.Equals("navi"))
            {
                playersNavi.Add(this);
            }
        }

        public static void CreatePlayers()
        {
            for(int i =0; i<5; i++)
            {
                new Player("g2");
            }

            for (int i = 0; i < 5; i++)
            {
                new Player("navi");
            }
        }

        private void SetPhoto(string team)
        {
            if (team.Equals("g2"))
            {
                this.photo = "~/images/players/g2/" + g2 + ".png";
                g2++;
            }
            else
            {
                this.photo = "~/images/players/navi/" + navi + ".png";
                navi++;
            }
        }
    }
}
