using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Tags;
using LoungeSaber.Models.Map;
using SiraUtil.Logging;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LoungeSaber.UI.BSML.Components
{
    public class VotingOption
    {
        private readonly SiraLog _siraLog = null;
        
        [UIComponent("coverImage")] 
        private readonly Image _coverImage = null;
        
        [UIValue("songNameText")]
        private string _songNameText { get; set; }
        
        [UIValue("songAuthorText")]
        private string _songAuthorText { get; set; }
        
        [UIValue("mapDifficultyText")]
        private string _mapDifficultyText { get; set; }

        public readonly VotingMap Map;
        
        public BeatmapLevel BeatmapLevel => Map.GetBeatmapLevel() ?? throw new Exception($"BeatmapLevel {Map.Hash} is not installed!");

        public VotingOption(VotingMap map, SiraLog siraLog)
        {
            _siraLog = siraLog;
            
            Map = map;

            var beatmapLevel = Map.GetBeatmapLevel();
            
            _songAuthorText = beatmapLevel?.songAuthorName;
            _songNameText = beatmapLevel?.songName;

            _mapDifficultyText = $"{map.Difficulty.ToString()} - {map.Category.ToString()}";
        }

        [UIAction("refresh-visuals")]
        async void RefreshVisuals(bool selected, bool highlighted)
        {
            try
            {
                var coverTexture = await BeatmapLevel.previewMediaData.GetCoverSpriteAsync();

                if (coverTexture is null) 
                    return;

                _coverImage.sprite = coverTexture;
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }
}