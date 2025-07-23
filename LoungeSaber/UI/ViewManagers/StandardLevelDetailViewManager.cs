using BeatSaberMarkupLanguage;
using JetBrains.Annotations;
using LoungeSaber.Models.Map;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.ViewManagers;

public class StandardLevelDetailViewManager : ViewManager, IInitializable, IDisposable
{
    [field: Inject]
    public StandardLevelDetailViewController ManagedController { get; } = null;
    
    public event Action<VotingMap, List<VotingMap>> OnMapVoteButtonPressed;

    private List<VotingMap> _votingMaps;
    private VotingMap _currentVotingMap;
    
    public void SetData(VotingMap votingMap, List<VotingMap> votingMaps)
    {
        _currentVotingMap = votingMap;
        _votingMaps = votingMaps;
        
        ManagedController.SetData(votingMap.GetBeatmapLevel(), true, "Vote", votingMap.GetBaseGameDifficultyType(), new BeatmapCharacteristicSO[]{});
    }

    public void Initialize()
    {
        ManagedController._standardLevelDetailView.actionButton.onClick.AddListener(OnActionButtonPressed);
    }

    private void OnActionButtonPressed() => OnMapVoteButtonPressed?.Invoke(_currentVotingMap, _votingMaps);

    public void Dispose()
    {
        ManagedController._standardLevelDetailView.actionButton.onClick.RemoveListener(OnActionButtonPressed);
    }
}