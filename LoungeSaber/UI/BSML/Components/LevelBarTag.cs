using BeatSaberMarkupLanguage.Tags;
using HMUI;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LoungeSaber.UI.BSML.Components;

public class LevelBarTag : BSMLTag
{
    public override GameObject CreateObject(Transform parent)
    {
        var containerObject = new GameObject("LevelBar", typeof(LayoutElement));

        var containerTransform = (RectTransform) containerObject.transform;
        containerTransform.SetParent(parent, false);
        containerTransform.anchorMin = Vector2.zero;
        containerTransform.anchorMax = Vector2.zero;
        containerTransform.sizeDelta = Vector2.zero;
        containerTransform.anchoredPosition = Vector2.zero;

        var levelBar = containerObject.AddComponent<CustomLevelBar>();
        levelBar.Init(parent);
        levelBar.LevelBar._showDifficultyAndCharacteristic = true;
        
        return containerObject;
    }

    public override string[] Aliases => ["level-bar"];
}