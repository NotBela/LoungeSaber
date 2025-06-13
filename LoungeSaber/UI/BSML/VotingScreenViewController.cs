using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.UI.BSML.Components;
using SiraUtil.Logging;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.VotingScreenView.bsml")]
    public class VotingScreenViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly SiraLog _siraLog = null;
        
        [UIValue("opponentText")] private string _opponentText { get; set; }
        
        [UIComponent("VotingMapList")] private readonly CustomCellListTableData _votingMapList = null;

        public void PopulateData(MatchCreatedPacket packet)
        {
            try
            {
                _opponentText = $"{packet.Opponent.GetFormattedBadgeName()} - {packet.Opponent.Mmr} MMR";

                var maps = new List<VotingOption>();

                foreach (var map in packet.Maps)
                {
                    var cellData = new VotingOption(map.GetBeatmapLevel(), map.Category, map.Difficulty, _siraLog);
                    maps.Add(cellData);
                }
                _votingMapList.Data = maps;
                _votingMapList.TableView.ReloadData();
            
                NotifyPropertyChanged(null);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }
}