using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.Sound;

public class SoundEffectManager(
    ResultsViewController resultsViewController,
    SongPreviewPlayer songPreviewPlayer,
    CountdownController countdownController)
{
    private readonly AudioClip _levelClearedAudioClip = resultsViewController._levelClearedAudioClip;
    private readonly AudioSource _gongAudioSource = countdownController._audioSource;
    
    [Inject] private readonly AudioClipAsyncLoader _audioClipAsyncLoader = null;
    [Inject] private readonly PerceivedLoudnessPerLevelModel _perceivedLoudnessPerLevelModel = null;

    public void PlayWinningMusic()
    {
        songPreviewPlayer.CrossfadeTo(_levelClearedAudioClip, -4f, 0f, _levelClearedAudioClip.length, null);
    }

    public void CrossfadeToDefault()
    {
        songPreviewPlayer.CrossfadeToDefault();
    }

    public void PlayGongSoundEffect()
    {
        // _gongAudioSource.Play();
    }

    public void PlayBeatmapLevelPreview(BeatmapLevel level) => PlayBeatmapLevelPreviewAsync(level);

    private async Task PlayBeatmapLevelPreviewAsync(BeatmapLevel level)
    {
        var clip = await _audioClipAsyncLoader.LoadPreview(level);

        var perceivedLoudness = _perceivedLoudnessPerLevelModel.GetLoudnessCorrectionByLevelId(level.levelID);
        
        songPreviewPlayer.CrossfadeTo(clip, perceivedLoudness, level.previewStartTime, level.previewDuration, () =>
        {
            _audioClipAsyncLoader.Unload(clip);
        });
    }
}