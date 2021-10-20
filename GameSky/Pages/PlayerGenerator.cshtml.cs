using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using GameSky.Proccessors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Pages
{
    public class PlayerGeneratorModel : PageModel
    {
        static Random random = new Random();
        private readonly DataContext db;
        private SignInManager<ApplicationUser> signInManager { get; set; }
        private UserManager<ApplicationUser> userManager { get; set; }
        public INotyfService Notyf { get; }
        public List<Player> activeMarket = null;
        public PlayerGeneratorModel(DataContext db, UserManager<ApplicationUser> userManager, INotyfService notyf)
        {
            this.db = db;
            Notyf = notyf;
            this.userManager = userManager;
        }


        public IActionResult OnGet(){
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var _user = userManager.GetUserAsync(currentUser).Result;

            if (_user.OwningTeamId is null)
            {
                Notyf.Error("Musisz posiadaæ dru¿ynê aby otrzymaæ dostêp do marketu.");
                return RedirectToPage("/Index");
            }
                var players = db.GetPlayersForMarket(_user.Id);
                if (players is not null && players.Count() == 5)
                {
                    activeMarket = players;
                }
            return Page();
        }

        public async Task<PartialViewResult> OnGetGeneratedPlayers()
        {

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var _user = userManager.GetUserAsync(currentUser).Result;

            List<Player> players = db.GetPlayersForMarket(_user.Id);
            
            if (players is not null && players.Count() == 5)
            {
                return Partial("~/Pages/Shared/PartialViews/_PlayersGenerator.cshtml", players);
            }
            //If there is no active market for user, generate new one
            for (int i = 0; i < 5; i++)
            {
                players.Add(Player.GeneratePlayer(db));
            }
            db.AddRange(players);
            var result = db.SaveChanges();
            if(result > 1)
            {
                var ed = DateTime.Now.AddDays(7);
                foreach (var player in players)
                {
                    MarketPlayer mp = new()
                    {
                        Player = player,
                        PlayerID = player.PlayerID,
                        User = _user,
                        UserID = _user.Id,
                        IsBought = false,
                        IsHidden = true,
                        ExpireDate = ed
                    };
                    db.Add(mp);
                }
                db.SaveChanges();
            }
            else
            {
                Notyf.Error("Nie uda³o siê wygenerowaæ zawodników.");
            }

            return Partial("~/Pages/Shared/PartialViews/_PlayersGenerator.cshtml", players);
        }

        public async Task<ActionResult> OnPostRerollMarket()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var _user = userManager.GetUserAsync(currentUser).Result;

            if(_user.Gold - 1500 >= 0)
            {
                _user.Gold -= 1500;
                List<MarketPlayer> marketPlayers = db.GetMarketPlayersByUserID(_user.Id);
                var ed = DateTime.Now.AddSeconds(-30);
                foreach (var market in marketPlayers)
                {
                    market.ExpireDate = ed;
                }
                db.SaveChanges();
                Notyf.Success("Wygenerowany nowy market!");
                return OnGetGeneratedPlayers().Result;
            }
            else
            {
                Notyf.Error("Nie masz wystarczaj¹co œrodków!");
                return new JsonResult(new { })
                {
                    StatusCode = 200,
                    Value = "error"
                };
            }

        }

        public async Task OnPostShowCard(int playerId)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var _user = userManager.GetUserAsync(currentUser).Result;
            var _player = db.GetPlayerByIdIncludePositon(playerId).Result;
            var mp = db.GetMarketForPlayer(_player.PlayerID, _user.Id);

            mp.IsHidden = false;

            var result = db.SaveChanges();
            if(result < 0)
            {
                Notyf.Error("Nie uda³o siê aktywowaæ karty");
            }
            else
            {
                Notyf.Information("Aktywowano karte!");
            }
        }

        public JsonResult OnPostBuyPlayer(int playerId)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var _user = userManager.GetUserAsync(currentUser).Result;
            var _player = db.GetPlayerByIdIncludePositon(playerId).Result;
            if (_user is null || _player is null)
            {
                return new JsonResult(new { })
                {
                    StatusCode = 400,
                    Value = "Nie znaleziono uzytkownika lub gracza"
                };
            }

            var mp = db.GetMarketForPlayer(_player.PlayerID, _user.Id);
            mp.IsBought = true;

            int prize = (int)_player.Prize;
            var output = new JsonResult(new {statusText = "error"}) 
            {
                StatusCode = 200,
                Value = "error",
                
            };

            if ((_user.Gold - prize) >= 0)
            {
                _user.Gold -= prize;

                //connect player to user's team
                var pt = new PlayerTeam
                {
                    Player = _player,
                    PlayerID = _player.PlayerID,
                    Team = db.GetTeamByID((int)_user.OwningTeamId),
                    TeamID = (int)_user.OwningTeamId,
                    JoinDate = DateTime.Now,
                    ExitDate = null,
                    IsOnTransfer = true,
                    IsInActiveRoster = false
                };
                db.Add(pt);

                var result = db.SaveChanges();
                if (result > 1 || result == 0)
                {
                    string response = String.Format("Zakupi³eœ zawodnika {0} za {1}G", _player.NickName, prize);
                    Notyf.Success(response);
                    output = new JsonResult(new {})
                    {
                        StatusCode = 200,
                        Value = "success"
                    };
                }
            }
            else
            {
                Notyf.Error("Niewystarczaj¹co z³ota!\nAby dokonaæ zakup,u rozwa¿ zakup z³ota.");
            }
            return output;
        }
    }
}