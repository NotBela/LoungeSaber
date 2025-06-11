using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.UI.BSML.Components;
using UnityEngine;

namespace LoungeSaber.UI.BSML
{
    [ViewDefinition("LoungeSaber.UI.BSML.VotingScreenView.bsml")]
    public class VotingScreenViewController : BSMLAutomaticViewController
    {
        [UIValue("opponentText")] private string _opponentText;
        
        [UIComponent("VotingMapList")] private readonly CustomCellListTableData _votingMapList = null;

        public void PopulateData(MatchCreatedPacket packet)
        {
            _opponentText = $"{packet.Opponent.GetFormattedBadgeName()} - {packet.Opponent.Mmr} MMR";

            _votingMapList.Data = packet.Maps.Select(i => new VotingOption(i.GetBeatmapLevel(), i.Category, i.Difficulty)).ToList();
            _votingMapList.TableView.ReloadData();
        }
    }
}