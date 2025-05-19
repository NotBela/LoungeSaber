using System;
using System.Net.Http;
using LoungeSaber.Configuration;

namespace LoungeSaber.Server.Api
{
    public class LoungeSaberApi
    {
        private readonly HttpClient _httpClient;

        internal LoungeSaberApi(PluginConfig config)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(config.ServerIp)
            };
        }
        
        public async 
    }
}