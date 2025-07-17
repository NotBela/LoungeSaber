using System.Collections;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HarmonyLib;
using HMUI;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using SiraUtil.Logging;
using SongCore;
using Zenject;
using Object = UnityEngine.Object;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.VotingScreenView.bsml")]
    public class VotingScreenViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly SiraLog _siraLog = null;
        public event Action<List<VotingMap>, VotingMap> MapSelected;
        
        private List<VotingMap> _options = new();
        
        [UIValue("opponentText")] private string OpponentText { get; set; }
        
        [UIComponent("mapList")] private readonly CustomListTableData _mapListTableData = null;
        
        public bool IsSafeToPopulate => _mapListTableData is not null;
        
        public void PopulateData(MatchCreatedPacket packet)
        {
            OpponentText = $"{packet.Opponent.GetFormattedUserName()} - {packet.Opponent.Mmr} MMR";

            _options = packet.Maps.ToList();

            _mapListTableData.Data = packet.Maps.Select(_ => new CustomListTableData.CustomCellInfo("")).ToList();

            for (var i = 0; i < _options.Count; i++)
            {
                var cell = _mapListTableData.CellForIdx(_mapListTableData.TableView, i) as LevelListTableCell;

                cell?._songBpmText.gameObject.SetActive(true);
                cell?._songDurationText.gameObject.SetActive(true);
                cell?._promoBadgeGo.gameObject.SetActive(true);
                cell?._updatedBadgeGo.gameObject.SetActive(true);
                cell?._favoritesBadgeImage.gameObject.SetActive(true);
                cell?.transform.Find("BpmIcon").gameObject.SetActive(true);

                cell?.SetDataFromLevelAsync(_options[i].GetBeatmapLevel(), false, false, false, true);
            }

            _mapListTableData.TableView.ReloadData();

            NotifyPropertyChanged(null);
        }

        // [UIAction("OnMapListCellSelected")]
        private void OnMapListCellSelected(TableView _, LevelListTableCell vote)
        {
            try
            {
                MapSelected?.Invoke(_options, _options.First(i => vote._beatmapLevel == i.GetBeatmapLevel()));
                
                // TODO: do this when vote button is pressed instead
                // await _serverListener.SendPacket(new VotePacket(_options.IndexOf(vote.Map)));
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }
}