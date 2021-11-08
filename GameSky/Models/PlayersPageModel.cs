using EFDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameSky.Models
{
    public class PlayersPageModel
    {
        public List<Player> Players { get; set; }
        public int TotalAmount { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int GetAmountOfPages()
        {
            int pages = TotalAmount / ItemsPerPage;
            if(TotalAmount % ItemsPerPage > 0)
            {
                pages++;
            }
            return pages;
        }
        public bool HasNext()
        {
            if(CurrentPage + 1 > GetAmountOfPages())
            {
                return false;
            }
            return true;
        }

        public bool HasMoreThanNext()
        {
            if (CurrentPage + 2 > GetAmountOfPages())
                return false;
            return true;
        }

        public bool HasPrev()
        {
            if(CurrentPage - 1 < 1)
            {
                return false;
            }
            return true;
        }

        public bool HasLessThanPrev()
        {
            if (CurrentPage - 2 < 1)
            {
                return false;
            }
            return true;
        }
    }
}
