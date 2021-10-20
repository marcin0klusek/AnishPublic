using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Pages.Team
{
    [Authorize]
    public class ManageModel : PageModel
    {
        private readonly DataContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public EFDataAccessLibrary.Models.Team Team { get; set; }
        public INotyfService Notyf { get; }

        public ManageModel(DataContext db, INotyfService Notyf, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.Notyf = Notyf;
            this.userManager = userManager;
        }
        public IActionResult OnGet(int id)
        {
            Team = db.GetTeamByID(id);

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var _user = userManager.GetUserAsync(currentUser).Result;

            if(_user.OwningTeamId == Team.TeamID)
            {
                return Page();
            }
            Notyf.Warning("Nie jesteœ w³aœcicielem dru¿yny!", 3);
            return RedirectToPage("/Index");
        }
        public PartialViewResult OnGetPlayerDetail(int PlayerID)
        {
            var player = db.GetPlayerByIdIncludePositon(PlayerID).Result;
            return Partial("~/Pages/Shared/PartialViews/_PlayerManage.cshtml", player);
        }

        /* Akcje do zarz¹dzania zawodnikiem */
        public IActionResult OnPostQuickSell(int PlayerID)
        {
            var player = db.GetPlayerByIdIncludePositon(PlayerID).Result;
            db.Remove<Player>(player);
            var result = db.SaveChanges();
            if(result > 0)
            {
                Notyf.Success($"Sprzedano zawodnika {player.NickName} za {player.GetPrize()}G");

                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var _user = userManager.GetUserAsync(currentUser).Result;
                _user.Gold += (double)player.GetPrize();
                db.SaveChanges();
            }
            else
            {
                Notyf.Error("Akcja nie powiod³a siê.");
            }


            return new JsonResult(new { })
            {
                StatusCode = 200,
                Value = Url.Page("Manage")
            };
        }

        public JsonResult OnPostPutPlayerOnAuction(int PlayerID, Boolean Status)
        {
            var player = db.GetPlayerByIdIncludePositon(PlayerID).Result;
            player.IsForSale = Status;
            var result = db.SaveChanges();
            if (result > 0)
            {
                Notyf.Success($"Zmieniono status gracza {player.NickName} na aukcji.");
                return new JsonResult(new { })
                {
                    StatusCode = 200,
                    Value = "success"
                };
            }

            Notyf.Error("Akcja nie powiod³a siê.");
            return new JsonResult(new { })
            {
                StatusCode = 200,
                Value = "error"
            };
        }

        public IActionResult OnPostChangePlayerActiveStatus(int PlayerID, Boolean Status)
        {
            var player = db.PlayerTeam.Where(pt => pt.PlayerID == PlayerID && pt.ExitDate == null).FirstOrDefault();
            player.Player = db.GetPlayerByIdIncludePositon(PlayerID).Result;
            player.Team = db.GetTeamByID(player.TeamID);
            int activePlayers = player.Team.GetActiveRoster().Count;
            player.IsInActiveRoster = Status;
            bool toSave = true;

            //Player is moving to Active Roster
            if (Status)
            {
                player.Player.IsForSale = false;
                if (activePlayers >= 5)
                {
                    toSave = false;
                    Notyf.Error("Aktywny sk³ad jest pe³ny!");
                }
            }

            if (toSave)
            {
                Notyf.Success($"Przeniesiono zawodnika {player.Player.NickName}", 3);
                db.SaveChanges();
            }
            else
            {
                Notyf.Error("Akcja nie powiod³a siê.", 3);
            }

            return new JsonResult(new { })
            {
                StatusCode = 200,
                Value = Url.Page("Manage")
            };
        }
    }
}
