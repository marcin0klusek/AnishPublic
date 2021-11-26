using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GameSky.Models
{
    public class EmailModel
    {
        [Required(ErrorMessage = "Muszę wiedzieć z kim się kontaktować")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Wprowadź temat o którym chciałbyś porozmawaić")]
        public string Topic { get; set; }
        [Required(ErrorMessage = "Opisz swoje zagadnienie")]
        public string Text { get; set; }
    }
}
