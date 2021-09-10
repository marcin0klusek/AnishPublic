using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class Toaster
    {
        private readonly INotyfService _notyf;

        public Toaster(INotyfService notyf)
        {
            _notyf = notyf;
        }

        public void ShowInformation(string text)
        {
            _notyf.Information(text, 1);
        }

        public void Warning(string text)
        {
            _notyf.Warning(text, 1);
        }

        public void Error(string text)
        {
            _notyf.Error(text, 3);
        }
    }
}
