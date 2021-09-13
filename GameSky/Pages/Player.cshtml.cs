using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;

namespace GameSky.Pages
{
    public class PlayerModel : PageModel
    {
        private readonly DataContext _db;
        private INotyfService _notyf;
        private readonly IJSRuntime js;

        public Player _player { get; set; }
        public int PlayerID { get; set; }
        public List<Match> upcomingMatches { get; set; }
        public UserManager<ApplicationUser> UserManager { get; }

        public PlayerModel(DataContext db, INotyfService notyf, UserManager<ApplicationUser> userManager, IJSRuntime js)
        {
            _db = db;
            _notyf = notyf;
            UserManager = userManager;
            this.js = js;
        }
        public void OnGet(int id)
        {
            _player = _db.GetPlayerByIdIncludePositon(id).Result;
            upcomingMatches = _db.GetMatchesForPlayer(id);
            if (upcomingMatches is null) upcomingMatches = new List<Match>();
        }
        public async Task<PartialViewResult> OnPostUpgradeSkill(string name, int id)
        {
            _player = _db.GetPlayerByIdIncludePositon(id).Result;
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;


            if (currentUser is null)
            {
                _notyf.Error("Musi siê zalogowaæ do tej akcji.\nErrorSkillCode: 1");
                return ReturnHeader();
            }

            ApplicationUser user = await UserManager.GetUserAsync(currentUser);

            if (user is null)
            {
                _notyf.Error("Nie odnaleziono u¿ytkownika.\nErrorSkillCode: 2");
                return ReturnHeader();
            }

            int PtnlPrice = Player.GetSkillPrice(_player.Potencial);
            int AimPrice = Player.GetSkillPrice(_player.Aim);
            int KnwlPrice = Player.GetSkillPrice(_player.Knowledge);
            int skillChanged = -1;

            switch (name)
            {
                case "Potencial":
                    if (CanUpgradeSkill(_player.Potencial, user.Gold))
                    {
                        _player.Potencial = _player.Potencial + 1;
                        user.Gold -= PtnlPrice;
                        skillChanged = 1;
                    }
                    else
                    {
                        _notyf.Error("Za ma³o œrodków na zakup");
                    }
                    break;
                case "Aim":
                    if (CanUpgradeSkill(_player.Aim, user.Gold))
                    {
                        _player.Aim = _player.Aim + 1;
                        user.Gold -= AimPrice;
                        skillChanged = 2;
                    }
                    else
                    {
                        _notyf.Error("Za ma³o œrodków na zakup");
                    }
                    break;

                case "Knowledge":
                    if (CanUpgradeSkill(_player.Aim, user.Gold))
                    {
                        _player.Knowledge = _player.Knowledge + 1;
                        user.Gold -= KnwlPrice;
                        skillChanged = 3;
                    }
                    else
                    {
                        _notyf.Error("Za ma³o œrodków na zakup");
                    }
                    break;
            }

            int result = await _db.SaveChangesAsync();
            if (result < 0)
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
                _notyf.Error("Wyst¹pi³ problem z zapisem");
            }
            else if(result > 1)
            {
                _player.RecalcPlayerStats();
                await _db.SaveChangesAsync();
            }
            //Should run javascript on client page to refresh amount of user's gold
            return ReturnHeader();
        }

        public JsonResult OnPostUsersGold()
        {

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            ApplicationUser user = _db.GetUserByName(currentUser.Identity.Name);
            if (user is null) return new JsonResult("-1");
            return new JsonResult(string.Format("{0:N}", user.Gold));
        }

        private bool CanUpgradeSkill(int skill, double gold)
        {
            double price = (double)Player.GetSkillPrice(skill);
            if (gold >= price) return true;
            return false;
        }

        private PartialViewResult ReturnHeader()
        {
            return Partial("~/Pages/Shared/PartialViews/_PlayerProfileHeader.cshtml", _player);
        }
    }
}
