using DoorToGate.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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
                var endpoint = $"flights?access_key=f415a6133de905c91e276fc3ef678ede";
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
