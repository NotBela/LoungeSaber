using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.Sound;

public class SoundEffectManager(
    SongPreviewPlayer songPreviewPlayer,
    ResultsViewController resultsViewController,
    CountdownController countdownController,
    LevelCollectionViewController levelCollectionViewController)
{
    private readonly AudioClip _levelClearedAudioClip = resultsViewController._levelClearedAudioClip;
    private readonly AudioSource _gongAudioSource = countdownController._audioSource;

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
        //todo: fix
        // _gongAudioSource.Play();
    }

    public void PlayBeatmapLevelPreview(BeatmapLevel level)
    {
        levelCollectionViewController.SongPlayerCrossfadeToLevel(level);
    }
}