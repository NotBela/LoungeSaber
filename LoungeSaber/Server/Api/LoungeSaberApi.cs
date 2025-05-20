using System;
using System.Net.Http;
using System.Threading.Tasks;
using LoungeSaber.Configuration;
using LoungeSaber.Models.Divisions;
using Newtonsoft.Json;
using SiraUtil.Logging;

namespace LoungeSaber.Server.Api
{
    public class LoungeSaberApi
    {
        private readonly SiraLog _siraLog = null;
        
        private readonly HttpClient _httpClient;
        
        public Division[] Divisions { get; private set; }

        internal LoungeSaberApi(PluginConfig config, SiraLog siraLog)
        {
            _siraLog = siraLog;
            
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri($"https://{config.ServerIp}:7198/")
            };
        }

        public async Task FetchDivisions()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/divisions");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var divisions = JsonConvert.DeserializeObject<Division[]>(json);
                Divisions = divisions;
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }
}