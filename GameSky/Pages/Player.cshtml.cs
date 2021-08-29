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
        public Player _player { get; set; }
        public int PlayerID { get; set; }


        public PlayerModel(DataContext db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            _player = _db.GetPlayerByIdIncludePositon(id);
        }
        public async Task<PartialViewResult> OnPostUpgradeSkill(string name, int id)
        {
            _player = _db.GetPlayerByIdIncludePositon(id);

            //check if User has enough money to pay

            int skillChanged = -1;
            switch (name)
            {
                case "Potencial":
                    _player.Potencial = _player.Potencial + 1;
                    skillChanged = 1;
                    break;
                case "Aim":
                    _player.Aim = _player.Aim + 1;
                    skillChanged = 2;
                    break;

                case "Knowledge":
                    _player.Knowledge = _player.Knowledge + 1;
                    skillChanged = 3;
                    break;
            }
            int result = await _db.SaveChangesAsync();
            if (result <= 0)
            {
                switch (skillChanged)
                {
                    case 1:
                        _player.Potencial = _player.Potencial - 1;
                        break;
                    case 2:
                        _player.Aim = _player.Aim - 1;
                        break;
                    case 3:
                        _player.Knowledge = _player.Knowledge - 1;
                        break;
                }
                StatusCode(500);
            }
            else
            {
                //Charge User for skill upgrade
            }
            return Partial("~/Pages/Shared/PartialViews/_PlayerProfileHeader.cshtml", _player);
        }
    }
}
