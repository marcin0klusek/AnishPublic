using AspNetCoreHero.ToastNotification.Abstractions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using GameSky.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Controllers
{
    [Authorize]
    [Controller]
    public class TicketController : Controller
    {
        public DataContext Db { get; }
        public INotyfService Notyf { get; }
        private UserManager<ApplicationUser> userManager { get; set; }
        public TicketController(DataContext db, INotyfService notyf, UserManager<ApplicationUser> userManager)
        {
            Db = db;
            Notyf = notyf;
            this.userManager = userManager;
        }
        [HttpGet]
        [Route("ticket/{id}")]
        public IActionResult Ticket(int id)
        {
            var ticket = Db.GetTicketById(id);

            System.Security.Claims.ClaimsPrincipal currentUser = HttpContext.User;
            var _user = userManager.GetUserAsync(currentUser).Result;

            bool IsAdmin = HttpContext.User.IsInRole("Admin");

            if (_user.Id != ticket.UserID && !IsAdmin)
            {
                Notyf.Error("Zgłoszenie nie należy do Ciebie.");
                return RedirectToPage("/Index");
            }
            ViewBag.Admin = IsAdmin;
            ViewBag.Ticket = ticket;
            return View("~/Views/Ticket.cshtml");
        }

        [HttpGet]
        [Route("ticket/create")]
        public IActionResult TicketCreate()
        {
            return View("~/Views/Ticket/Create.cshtml");
        }

        [HttpPost]
        [Route("ticket/create")]
        public async Task<IActionResult> OnPostCreateTicket(IFormCollection form)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = HttpContext.User;
            var _user = userManager.GetUserAsync(currentUser).Result;

            string subject = form["Subject"].ToString();
            string desc = form["Description"].ToString();
            var date = DateTime.Now;

            Ticket ticket = new();
            ticket.Subject = subject;
            ticket.Description = desc;
            ticket.Status = TicketStatus.New;
            ticket.CreateAt = date;
            ticket.LastModify = date;
            ticket.User = _user;
            ticket.UserID = _user.Id;

            Db.Add(ticket);
            var result = Db.SaveChanges();
            if (result > 0)
            {
                Notyf.Information("Dodano zgłoszenie.");
                TicketHub.NewTicket(ticket.Id, ticket.Subject, ticket.Status);

                return RedirectToAction("Ticket", ticket);
            }
            Notyf.Error("Wystąpił problem z dodawaniem zgłoszenia. Spróbuj ponownie za chwilę.");
            return RedirectToAction("Tickets");
        }

        [HttpPost]
        [Route("ticket/{id}")]
        public async Task<IActionResult> OnPostAddTicket(IFormCollection form)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = HttpContext.User;
            var _user = userManager.GetUserAsync(currentUser).Result;

            string text = form["comment"].ToString();
            var date = DateTime.Now;
            int ticketId = Int32.Parse(form["ticketId"].ToString());

            var c = new Comment
            {
                CreateAt = date,
                Description = text,
                TicketID = ticketId,
                User = _user,
                UserID = _user.Id
            };

            Db.Comments.Add(c);
            var ticket = Db.GetTicketById(ticketId);

            var tc = new TicketComment
            {
                Comment = c,
                Ticket = ticket
            };

            Db.TicketComments.Add(tc);
            ticket.LastModify = DateTime.Now;
            if (currentUser.IsInRole("Admin"))
            {
                ticket.Status = TicketStatus.Responded;
            }
            else
            {
                if(ticket.Status == TicketStatus.Responded)
                {
                    ticket.Status = TicketStatus.Needs_Attention;
                }
            }
            TicketHub.UpdateStatus(ticketId, ticket.Status);
            Db.Tickets.Update(ticket);

            var result = Db.SaveChanges();
            if (result > 0)
            {
                Notyf.Information("Dodano odpowiedź do zgłoszenia.");
            }
            else
            {
                Notyf.Error("Wystąpił problem z dodawaniem odpowiedzi. Spróbuj ponownie za chwilę.");
            }
            return RedirectToAction("Ticket", ticket);
        }
    }
}
