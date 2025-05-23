using System;
using System.Net.Http;
using System.Threading.Tasks;
using HarmonyLib;
using JetBrains.Annotations;
using LoungeSaber.Configuration;
using LoungeSaber.Models.Divisions;
using Newtonsoft.Json;
using SiraUtil.Logging;
using UnityEngine;

namespace LoungeSaber.Server.Api
{
    public class LoungeSaberApi
    {
        private readonly SiraLog _siraLog = null;
        
        private readonly HttpClient _httpClient;

        internal LoungeSaberApi(PluginConfig config, SiraLog siraLog)
        {
            _siraLog = siraLog;
            
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://{config.ServerIp}:7198/")
            };
        }

        [CanBeNull]
        public async Task<Division[]> FetchDivisions()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/divisions");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var divisions = JsonConvert.DeserializeObject<Division[]>(json);
                return divisions;
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }

            return null;
        }
        
        public event Action<Division[]> OnDivisionDataRefreshed;
        public event Action OnDivisionDataRefreshStarted;
    }
}