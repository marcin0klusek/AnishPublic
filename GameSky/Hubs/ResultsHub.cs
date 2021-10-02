using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFDataAccessLibrary.DataAccess;
using EFDataAccessLibrary.Models;
using Microsoft.AspNetCore.SignalR;

namespace GameSky.Hubs
{
    public class ResultsHub : Hub
    {
        public static IHubContext<ResultsHub> Current { get; set; }
    }
}
