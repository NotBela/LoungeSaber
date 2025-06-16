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
        
        [UIComponent("textVertical")] private readonly VerticalLayoutGroup _textVertical = null;

        public readonly BeatmapLevel ParentBeatmap;

        public VotingOption(BeatmapLevel beatmapLevel, VotingMap.CategoryType categoryType, VotingMap.DifficultyType difficulty, SiraLog siraLog)
        {
            _siraLog = siraLog;
            
            ParentBeatmap = beatmapLevel;
            
            _songAuthorText = beatmapLevel.songAuthorName;
            _songNameText = beatmapLevel.songName;

            _mapDifficultyText = $"{difficulty.ToString()} - {categoryType.ToString()}";
        }

        [UIAction("refresh-visuals")]
        async void RefreshVisuals(bool selected, bool highlighted)
        {
            try
            {
                Plugin.Log.Info("Loading cover image data");
            
                var coverTexture = await ParentBeatmap.previewMediaData.GetCoverSpriteAsync();

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