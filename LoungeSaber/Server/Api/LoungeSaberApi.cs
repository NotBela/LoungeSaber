using System;
using System.Net.Http;
using System.Threading.Tasks;
using LoungeSaber.Configuration;
using LoungeSaber.Models.Divisions;
using Newtonsoft.Json;

namespace LoungeSaber.Server.Api
{
    public class LoungeSaberApi
    {
        private readonly HttpClient _httpClient;
        
        public Division[] Divisions { get; private set; }

        internal LoungeSaberApi(PluginConfig config)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(config.ServerIp)
            };
        }

        public async Task FetchDivisions()
        {
            var response = await _httpClient.GetAsync("/api/divisions");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            var divisions = JsonConvert.DeserializeObject<Division[]>(json);
            Divisions = divisions;
        }
    }
}