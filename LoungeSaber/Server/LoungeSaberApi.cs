using System.Net;
using System.Net.Http;
using JetBrains.Annotations;
using LoungeSaber.Configuration;
using LoungeSaber.Models.Server;
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

        public async Task<Models.UserInfo.UserInfo[]> GetLeaderboardRange(int start, int range)
        {
            var response = await _client.GetAsync($"/api/leaderboard/range?start={start}&range={range}");
            
            return JsonConvert.DeserializeObject<Models.UserInfo.UserInfo[]>(await response.Content.ReadAsStringAsync());
        }

        [ItemCanBeNull]
        public async Task<Models.UserInfo.UserInfo[]> GetAroundUser(string id)
        {
            var response = await _client.GetAsync($"/api/leaderboard/aroundUser/{id}");
            return response.StatusCode == HttpStatusCode.NotFound ? null : JsonConvert.DeserializeObject<Models.UserInfo.UserInfo[]>(await response.Content.ReadAsStringAsync());
        }

        [ItemCanBeNull]
        public async Task<ServerStatus> GetServerStatus()
        {
            var response = await _client.GetAsync("/api/server/status");
            return response.StatusCode == HttpStatusCode.NotFound ? null : JsonConvert.DeserializeObject<ServerStatus>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string[]> GetMapHashes()
        {
            var response = await _client.GetAsync("/api/maps/hashes");
            return JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());
        }
    }
}