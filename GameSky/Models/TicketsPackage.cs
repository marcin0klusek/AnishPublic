using EFDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class TicketsPackage
    {
        public List<Ticket> TicketsNew { get; set; }
        public List<Ticket> TicketsToDo { get; set; }
        public List<Ticket> TicketsCompleted { get; set; }

        public TicketsPackage(List<Ticket> newTickets, List<Ticket> todoTickets, List<Ticket> completedTickets)
        {
            TicketsNew = newTickets;
            TicketsToDo = todoTickets;
            TicketsCompleted = completedTickets;
        }
    }
}
