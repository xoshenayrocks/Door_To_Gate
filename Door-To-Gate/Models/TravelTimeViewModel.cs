using System;
namespace Door_To_Gate.Models
{
    public class TravelTimeViewModel
    {
        public bool ArriveBy { get; set; }
        public string Time { get; set; }
        public string DriveTime { get; set; }
        public double TotalTravelTime { get; set; }
        public string Location { get; set; }
        public string AirportName { get; set; }
        public double TSAWaitTime { get; set; }
        public string LeaveTime { get; set; }
        public string Code { get; set; }

    }
}
