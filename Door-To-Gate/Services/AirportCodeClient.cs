using DoorToGate.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static DoorToGate.Models.AirportCode;

namespace DoorToGate.Services
{
    public class AirportCodeClient : IAirportCodeClient
    {
        private readonly HttpClient _client;

        public AirportCodeClient(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<AirportCode> GetAirportCode()
        {
            try
            {
  
                    var endpoint = $"airports?access_key=7ef0c4bfc64dfe51c3a0cbeef08edd19&country_name=United States&limit=5000";
                    var response = await _client.GetAsync(endpoint);
                    var json = await response.Content.ReadAsStringAsync();
                     return JsonSerializer.Deserialize<AirportCode>(json);

            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
    }
}
