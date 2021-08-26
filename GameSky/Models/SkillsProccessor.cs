using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class SkillsProccessor
    {
        public static int GetPriceToUpgradeSkill(int skillPoint)
        {
            switch (skillPoint)
            {
                case 0:
                    return 500;
                case 1: case 2: case 3:
                    return 1500;
                case 4: case 5:
                    return 3000;
                case 6: case 7:
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
    }
}
