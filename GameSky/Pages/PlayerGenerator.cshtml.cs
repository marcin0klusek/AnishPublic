using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using GameSky.Proccessors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Pages
{
    public class PlayerGeneratorModel : PageModel
    {
        static Random random = new Random();
        private readonly DataContext db;
        public PlayerGeneratorModel(DataContext db)
        {
            this.db = db;
        }
        public void OnGet(){}

        public async Task<PartialViewResult> OnGetGeneratedPlayers()
        {
            List<Player> players = new List<Player>();
            for (int i = 0; i < 5; i++)
            {
                players.Add(PlayerProccessor.GeneratePlayer(db));
            }

            return Partial("~/Pages/Shared/PartialViews/_PlayersGenerator.cshtml", players);
        }

        public async Task<IActionResult> OnPostBuyPlayer(int playerPos)
        {
            return new JsonResult("nikt" + playerPos);
        }
    }
}