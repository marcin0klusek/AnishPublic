using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class Country
    {

        private static string GenerateString(string kraj)
        {
            return "https://flagcdn.com/w20/" + kraj + ".png";
        }
        public static String GetCountry(String symbol)
        {
            return Country.GenerateString(symbol.ToLower());
        }
    }
}
