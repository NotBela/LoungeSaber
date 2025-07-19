﻿using System.Collections;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HarmonyLib;
using HMUI;
using JetBrains.Annotations;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using SiraUtil.Logging;
using SongCore;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace LoungeSaber.UI.BSML.Match;

[ViewDefinition("LoungeSaber.UI.BSML.Match.VotingScreenView.bsml")]
public class VotingScreenViewController : BSMLAutomaticViewController
{
    [Inject] private readonly SiraLog _siraLog = null;
    public event Action<VotingMap> MapSelected;
        
    private List<VotingMap> _options = new();
        
    [UIValue("opponentText")] private string OpponentText { get; set; }
        
    [UIComponent("mapList")] private readonly CustomListTableData _mapListTableData = null;
        
    private VotingListDataSource _votingListDataSource = null!;

    [UIAction("#post-parse")]
    void PostParse()
    {
        _votingListDataSource = gameObject.AddComponent<VotingListDataSource>();
        _mapListTableData.TableView.SetDataSource(_votingListDataSource, true);
        
        _votingListDataSource.Init(_mapListTableData.TableView);
        
        Destroy(_mapListTableData);
    }

    public void PopulateData(MatchCreatedPacket packet)
    {
        _options = packet.Maps.ToList();
        _votingListDataSource.SetData(packet.Maps.ToList());
        
        OpponentText = $"{packet.Opponent.GetFormattedUserName()} - {packet.Opponent.Mmr} MMR";
        
        NotifyPropertyChanged(null);
    }
}
    
public class VotingListDataSource : MonoBehaviour, TableView.IDataSource
{
    public TableView TableView { get; private set; }
        
    public List<VotingMap> Data { get; private set; } = new();

    private LevelListTableCell _tableCellPrefab;

    private LevelListTableCell CreateTableCellPrefab()
    {
        var gameObj = Instantiate(
            Resources.FindObjectsOfTypeAll<LevelCollectionViewController>()
                .First()
                .transform
                .Find("LevelsTableView/TableView/Viewport/Content/LevelListTableCell")
                .gameObject);
            
        gameObj.name = "MyListCell";

        var cell = gameObj.GetComponent<LevelListTableCell>();
        return cell;
    }
        
    public void Init(TableView tableView) => TableView = tableView;

    public void SetData(List<VotingMap> maps)
    {
        Data = maps;
        TableView.ReloadData();
    }

    public float CellSize(int idx) => 8.5f;

    public int NumberOfCells() => Data.Count;

    public TableCell CellForIdx(TableView tableView, int idx)
    {
        var cell = (LevelListTableCell) tableView.DequeueReusableCellForIdentifier("VotingListTableCell");

        if (cell is null)
        {
            _tableCellPrefab ??= CreateTableCellPrefab();
            cell = Instantiate(_tableCellPrefab);
            cell.reuseIdentifier = "VotingListTableCell";
        }

        var info = Data[idx];
        cell.SetDataFromLevelAsync(info.GetBeatmapLevel(), false,false, false, true);

        return cell;
    }
}