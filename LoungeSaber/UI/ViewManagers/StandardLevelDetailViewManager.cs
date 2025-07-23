using HMUI;
using LoungeSaber.Models.Map;
using Zenject;

namespace LoungeSaber.UI.ViewManagers;

public class StandardLevelDetailViewManager : ViewManager
{
    [Inject] private readonly StandardLevelDetailViewController _standardLevelDetailViewController = null;
    
    public override ViewController ManagedController => _standardLevelDetailViewController;
    
    public event Action<VotingMap, List<VotingMap>> OnMapVoteButtonPressed;

    private List<VotingMap> _votingMaps;
    private VotingMap _currentVotingMap;
    
    public void SetData(VotingMap votingMap, List<VotingMap> votingMaps)
    {
        _currentVotingMap = votingMap;
        _votingMaps = votingMaps;
        
        _standardLevelDetailViewController.SetData(votingMap.GetBeatmapLevel(), true, "Vote", votingMap.GetBaseGameDifficultyType(), votingMap.GetBeatmapLevel()?.beatmapBasicData.Keys.Select(i => i.characteristic).Where(i => i.serializedName != "Standard").ToArray());

        var dataSource = new SegmentedControlDataSource();
        
        _standardLevelDetailViewController._standardLevelDetailView._beatmapCharacteristicSegmentedControlController._segmentedControl.dataSource = dataSource;

        var cell = gameObject.AddComponent<TextSegmentedControlCell>();
        
        dataSource.SetSegmentedControlCells(new List<SegmentedControlCell> {cell});

        cell.text = $"Category: {votingMap.Category}";
        
        _standardLevelDetailViewController._standardLevelDetailView._beatmapCharacteristicSegmentedControlController._segmentedControl.ReloadData();
    }

    protected override void SetupManagedController()
    {
        _standardLevelDetailViewController._standardLevelDetailView.actionButton.onClick.AddListener(OnActionButtonPressed);
    }

    private void OnActionButtonPressed() => OnMapVoteButtonPressed?.Invoke(_currentVotingMap, _votingMaps);

    protected override void ResetManagedController()
    {
        _standardLevelDetailViewController._standardLevelDetailView.actionButton.onClick.RemoveListener(OnActionButtonPressed);
    }
}

public class SegmentedControlDataSource : SegmentedControl.IDataSource
{
    private List<SegmentedControlCell> cells = new();

    public void SetSegmentedControlCells(List<SegmentedControlCell> list)
    {
        cells = list;
    }

    public int NumberOfCells() => cells.Count;

    public SegmentedControlCell CellForCellNumber(int cellNumber) => cells[cellNumber];
}