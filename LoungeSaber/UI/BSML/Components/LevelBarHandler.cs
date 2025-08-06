using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.TypeHandlers;
using SongCore;

namespace LoungeSaber.UI.BSML.Components;

[ComponentHandler(typeof(LevelBar))]
public class LevelBarHandler : TypeHandler<CustomLevelBar>
{

    public override Dictionary<string, Action<CustomLevelBar, string>> Setters => new()
    {
        { "beatmapHash", (component, value) =>
            {
                if (value == "")
                {
                    return;
                }
                
                component.SetupBeatmap(Loader.GetLevelByHash(value));
            }
        },
        { "difficulty", (component, value) => component.SetDifficultyText(value)}
    };

    public override Dictionary<string, string[]> Props => new()
    {
        { "beatmapHash", ["beatmap-hash"] },
        { "difficulty", ["difficulty"] }
    };
}