using System;
namespace Door_To_Gate.Models
{
    public class TravelTimeViewModel
    {
        public bool ArriveBy { get; set; }
        public DateTime Time { get; set; }
        public int TotalTravelTime { get; set; }
        public string Location { get; set; }
        public string AirportName { get; set; }
    }
}
