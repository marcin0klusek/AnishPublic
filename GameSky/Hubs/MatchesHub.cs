using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using GameSky.Models;

namespace GameSky.Hubs
{
    public class MatchesHub : Hub
    {
        public static IHubContext<MatchesHub> Current { get; set; }
        private static Dictionary<string, int> UserAmountPerGroup = new();

        public async Task JoinRoom(string roomName)
        {
            Console.WriteLine($"Połączono {Context.User.Identity.Name} do {roomName}.");
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("PrintInConsole", $"User joined to {roomName}.");
            ChangeAmount(roomName, 1);
        }

        public async Task LeaveRoom(string roomName)
        {
            Console.WriteLine($"Rozłączono {Context.User.Identity.Name} z {roomName}.");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("PrintInConsole", $"User left {roomName}.");
            ChangeAmount(roomName, -1);
        }

        public void ChangeAmount(string roomName, int x)
        {
            if (UserAmountPerGroup.ContainsKey(roomName))
            {
                UserAmountPerGroup[roomName] += x;
            }
            else
            {
                UserAmountPerGroup.Add(roomName, x);
            }
            Console.WriteLine($"Ilosc uzytkownikow dla {roomName} wynosi: {UserAmountPerGroup[roomName]}");
        }

        public async Task CreateAction(string roomName)
        {
            MatchAction action = new()
            {
                Type = MatchActionEnum.Kill,
                Player1 = "Player1",
                Player2 = "Player2"
            };

            await Clients.Group(roomName).SendAsync("NewAction", $"{action.Player1} {action.Type.ToString()} {action.Player2}");
        }
    }
}
