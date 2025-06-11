using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using LoungeSaber.Models.Map;
using UnityEngine.UI;

namespace LoungeSaber.UI.BSML.Components
{
    public class VotingOption
    {
        [UIComponent("coverImage")] 
        private readonly RawImage _coverImage = null;
        
        [UIValue("songNameText")]
        private readonly string _songNameText;
        
        [UIValue("songAuthorText")]
        private readonly string _songAuthorText;
        
        [UIValue("mapCategoryText")]
        private readonly string _mapCategoryText;
        
        [UIValue("mapDifficultyText")]
        private readonly string _mapDifficultyText;

        public readonly BeatmapLevel ParentBeatmap;

        public VotingOption(BeatmapLevel beatmapLevel, VotingMap.CategoryType categoryType, VotingMap.DifficultyType difficulty)
        {
            ParentBeatmap = beatmapLevel;
            
            _songAuthorText = beatmapLevel.songAuthorName;
            _songNameText = beatmapLevel.songName;
            
            _mapCategoryText = categoryType.ToString();
            _mapDifficultyText = difficulty.ToString();
        }

        public async Task LoadCoverImageData()
        {
            _coverImage.texture = (await ParentBeatmap.previewMediaData.GetCoverSpriteAsync()).texture;
        }
    }
}