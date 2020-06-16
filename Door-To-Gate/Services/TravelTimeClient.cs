using System;
namespace Door_To_Gate.Services
{
    public class TravelTimeClient
    {
        public static string HoursMins (double duration)
        {
            TimeSpan timeInHrsMins = TimeSpan.FromHours(duration);
            string displayTime = string.Empty;
            if (timeInHrsMins.Hours > 0)
                displayTime += timeInHrsMins.Hours.ToString() + " hours";

            if (timeInHrsMins.Minutes > 0)
                displayTime += timeInHrsMins.Minutes.ToString() + " minutes";

            return displayTime;
        }
       
    }
}
