using DoorToGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToGate.Services
{
    public interface IAirportCodeClient
    {
        Task<AirportCode> GetAirportCode();
    }
}