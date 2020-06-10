using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Door_To_Gate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DoorToGate.Models;
using DoorToGate.Services;

namespace DoorToGate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITSAClient _airportClient;
        private readonly IAirportCodeClient _airportCodeClient;

        public HomeController(ITSAClient airportClient, IAirportCodeClient airportCodeClient)
        {
            _airportClient = airportClient;
            _airportCodeClient = airportCodeClient;
        }

        public async Task<IActionResult> Index()
        {
            var clientResult = await _airportCodeClient.GetAirportCode();

            return View(clientResult);
        }

        

        public async Task<IActionResult> GetWaitTime(string airport)
        {
            var clientResult = await _airportClient.GetAirport(airport);

            return View(clientResult);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
