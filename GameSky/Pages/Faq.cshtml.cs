using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSky.Pages
{
    public class FaqModel : PageModel
    {
        private readonly DataContext _db;
        private INotyfService _notyf;

        public Faq Faq { get; set; }
        public List<Question> Questions { get; set; }
        public FaqModel(DataContext db, INotyfService notyf)
        {
            _db = db;
            _notyf = notyf;
        }

        public IActionResult OnGet()
        {
            Faq = _db.GetActiveFaq();
            if (Faq is null) { 
                _notyf.Warning("Nie odnaleziono aktywnego FAQ.");
                return RedirectToPage("/Index");
            }
            Questions = _db.GetQuestionsForFaq(Faq.FaqID);
            return Page();
        }
    }
}
