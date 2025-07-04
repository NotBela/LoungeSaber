using System.Net;
using System.Net.Http;
using JetBrains.Annotations;
using LoungeSaber.Configuration;
using Newtonsoft.Json;
using Zenject;

namespace LoungeSaber.Server
{
    public class LoungeSaberApi : IInitializable
    {
        [Inject] private readonly PluginConfig _config = null!;
        
        private readonly HttpClient _client = new();

        public void Initialize()
        {
            _client.BaseAddress = new Uri($"https://{_config.ServerIp}:{_config.ServerApiPort}/");
        }

        [ItemCanBeNull]
        public async Task<Models.UserInfo.UserInfo> GetUserInfo(string id)
        {
            var response = await _client.GetAsync($"/api/user/id/{id}");

            return response.StatusCode == HttpStatusCode.NotFound ? null : JsonConvert.DeserializeObject<Models.UserInfo.UserInfo>(await response.Content.ReadAsStringAsync());
        }
    }
}