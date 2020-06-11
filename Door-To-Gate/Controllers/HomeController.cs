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
            var clientResult = await _airportClient.GetAirport(model.AirportName);

            return View(clientResult);
        }

        public IActionResult TravelTime(AirportCode model)
        {
            string time = Request.Query["time"];
            model.AirportName = Request.Form["airport"];
            string hourMin = time.Substring(0, 5);
            time = DateTime.Parse(hourMin).ToString("hh:mm tt");
            model.Time = Convert.ToDateTime(time);
            DirectionsRequest request = new DirectionsRequest();
            request.Key = "AIzaSyAD_-v70Gc1IQ2mfHkKTjCYBINKMlQ4I8I";
            request.Origin = new Location(model.Location);
            request.Destination = new Location(model.AirportName);
            var response =  GoogleApi.GoogleMaps.Directions.Query(request);

            double duration = response.Routes.First().Legs.First().Duration.Value / 60;

            if (model.ArriveBy)
            {
                DateTime arriveby = model.Time;
                DateTime updated = arriveby.AddMinutes(duration);
                //logic for time it takes to get to airport.. are we introducing Google API here?
                //to arrive by a certain time, take time it takes to get to airport selected (using Google API?) from user's location
                //add or subtract that to user's current local time (and TSA wait time)
                /*ex. if I want to arrive to DTW by 5pm and I am viewing the app at 12pm (say TSA wait time is 30 mins), I will need
                  to leave by 4:10pm )

                    DateTime arriveby = new DateTime(year, month, day, 17, 0, 0);
                    DateTime updated = original.AddMinutes(-50));  <<subtracts 20 min drive time & 30 min wait time from time
                            user states they want to arrive by*/

                //display time needed to leave in order to arrive to airport by designated time
            }
            else
            {

            }
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
