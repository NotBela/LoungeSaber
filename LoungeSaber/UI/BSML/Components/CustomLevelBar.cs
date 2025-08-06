using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;
using Object = System.Object;

namespace LoungeSaber.UI.BSML.Components;

public class CustomLevelBar : MonoBehaviour
{
    public LevelBar LevelBar { get; private set; }
    
    public void Init(Transform parent)
    {
        LevelBar = Instantiate(Resources.FindObjectsOfTypeAll<ResultsViewController>().First()._levelBar, parent, false);
        LevelBar._difficultyText.gameObject.SetActive(true);
        LevelBar._characteristicIconImageView.gameObject.SetActive(true);
    }

    public void SetupBeatmap(BeatmapLevel beatmapLevel)
    {
        LevelBar.Setup(beatmapLevel.GetBeatmapKeys().First(i => i.beatmapCharacteristic.serializedName == "Standard"));
        LevelBar._difficultyText.text = "";
    }

    public void SetDifficultyText(string text)
    {
        LevelBar._difficultyText.text = text;
    }
}