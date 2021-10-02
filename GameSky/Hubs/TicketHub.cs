using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.SignalR;

namespace GameSky.Hubs
{
    public class TicketHub : Hub
    {
        public static IHubContext<TicketHub> Current { get; set; }

        public static void UpdateStatus(int TicketID, TicketStatus status)
        {
            Current.Clients.All.SendAsync("UpdateStatus", TicketID, status.ToString());
        }

        public static void NewTicket(int TicketID, string subject, TicketStatus status)
        {
            Current.Clients.All.SendAsync("NewTicket", TicketID, subject, status.ToString());            
        }

        public static void CompleteTicket(int TicketID)
        {
            Current.Clients.All.SendAsync("CompleteTicket", TicketID);            
        }
    }
}
