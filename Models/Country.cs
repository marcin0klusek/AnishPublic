using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class Country
    {
        private static Random rnd = new Random();
        private static List<String> countries = new List<string>()
        {
            "GM", "GN", "PL", "ID", "IE", "IL", "IR", "IS", "GB", "ES", "EH", "DM", "DO", "CZ", "CN", "CI", "CK", "BR", "BE", "BA", "SV", "TD", "TM", "FR", "UA" 
        };

        private static string GenerateString(string kraj)
        {
            return "https://www.countryflags.io/" + kraj + "/shiny/24.png";
        }

        public static String GetCountry()
        {
            return Country.GenerateString(countries[rnd.Next(0, countries.Count)]);
        }
        public static String GetCountry(String symbol)
        {
            foreach(string country in countries)
            {
                if (country.Equals(symbol))
                {
                    return Country.GenerateString(country); ;
                }
            }
            return GetCountry();
        }
    }
}
