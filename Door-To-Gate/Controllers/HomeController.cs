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
using GoogleApi.Entities.Maps.Directions.Request;
using GoogleApi.Entities.Common;

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


        public async Task<IActionResult> GetWaitTime(AirportCode model)

        {
            var clientResult = await _airportClient.GetAirport(model.Code);

            return View(clientResult);
        }

        public async Task<IActionResult> TravelTime(AirportCode model)
        {
            TravelTimeViewModel travel = new TravelTimeViewModel();
            travel.ArriveBy = model.ArriveBy;
            string time = Request.Form["time"];
            model.Code = Request.Form["airport"];
            var tsaWaitTime = await _airportClient.GetAirport(model.Code);
            travel.TSAWaitTime = tsaWaitTime.rightnow/60;
            string hourMin = time.Substring(0, 5);
            time = DateTime.Parse(hourMin).ToString("h:mm tt");
            model.Time = Convert.ToDateTime(time);
            travel.Time = model.Time;
            DirectionsRequest request = new DirectionsRequest();
            request.Key = "AIzaSyAD_-v70Gc1IQ2mfHkKTjCYBINKMlQ4I8I";
            request.Origin = new Location(model.Location);
            request.Destination = new Location(model.Code);
            var response =  GoogleApi.GoogleMaps.Directions.Query(request);

            travel.DriveTime = response.Routes.First().Legs.First().Duration.Value / 3600D;
            travel.TotalTravelTime = travel.DriveTime + travel.TSAWaitTime;

            if (model.ArriveBy)
            {
                DateTime arriveByTime = model.Time;
                DateTime updatedTime = arriveByTime.AddHours(-(travel.TotalTravelTime));
                travel.LeaveTime = updatedTime.ToString("h:mm tt");
            }
            else
            {
      
            }

            ;
            return View();
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
