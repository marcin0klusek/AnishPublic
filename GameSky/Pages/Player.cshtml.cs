using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using GameSky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Pages
{
    public class PlayerModel : PageModel
    {
        private readonly DataContext _db;
        public Player player { get; set; }
        public int PotencialPrice;
        public int AimPrice;
        public int KnowledgePrice;


        public PlayerModel(DataContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            player = _db.Player.Where(p => p.PlayerID == id).FirstOrDefault();

            if (player == null) { RedirectToPage("/Index"); }

            PotencialPrice = SkillsProccessor.GetPriceToUpgradeSkill(player.Potencial);
            AimPrice = SkillsProccessor.GetPriceToUpgradeSkill(player.Aim);
            KnowledgePrice = SkillsProccessor.GetPriceToUpgradeSkill(player.Knowledge);
        }

        public async Task UpgradeSkill(string name)
        {
            //check if User has enough money to pay
            int skillChanged = -1;
            switch (name)
            {
                case "Potencial":
                    player.Potencial = player.Potencial++;
                    skillChanged = 1;
                    break;
                case "Aim":
                    player.Potencial = player.Aim++;
                    skillChanged = 2;
                    break;

                case "Knowledge":
                    player.Potencial = player.Knowledge++;
                    skillChanged = 3;
                    break;
            }
            int result = await _db.SaveChangesAsync();
            if(result <= 0)
            {
                switch (skillChanged)
                {
                    case 1:
                        player.Potencial = player.Potencial--;
                        break;
                    case 2:
                        player.Potencial = player.Aim--;
                        break;
                    case 3:
                        player.Potencial = player.Knowledge--;
                        break;
                }
            }
            else
            {
                //Charge User for skill upgrade
            }
            Response.Redirect(Request.Path + "?id=" + player.PlayerID);
        }
    }
}
