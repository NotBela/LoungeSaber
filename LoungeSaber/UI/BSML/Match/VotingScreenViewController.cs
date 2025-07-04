using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.UserPackets;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.Components;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.UI.BSML.Match
{
    [ViewDefinition("LoungeSaber.UI.BSML.Match.VotingScreenView.bsml")]
    public class VotingScreenViewController : BSMLAutomaticViewController
    {
        [Inject] private readonly SiraLog _siraLog = null;
        [Inject] private readonly ServerListener _serverListener = null;
        public event Action<List<VotingMap>, VotingMap> MapSelected;
        
        private List<VotingMap> _options = new List<VotingMap>();
        
        [UIValue("opponentText")] private string _opponentText { get; set; }
        
        [UIComponent("VotingMapList")] private readonly CustomCellListTableData _votingMapList = null;

        public void PopulateData(MatchCreatedPacket packet)
        {
            try
            {
                _opponentText = $"{packet.Opponent.GetFormattedUserName()} - {packet.Opponent.Mmr} MMR";

                _options = packet.Maps.ToList();

                _votingMapList.Data = packet.Maps.Select(map => new VotingOption(map, _siraLog)).ToList();
                _votingMapList.TableView.ReloadData();
            
                NotifyPropertyChanged(null);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }

        [UIAction("OnMapListCellSelected")]
        private async void OnMapListCellSelected(TableView _, VotingOption vote)
        {
            try
            {
                MapSelected?.Invoke(_options, vote.Map);

                await _serverListener.SendPacket(new VotePacket(_options.IndexOf(vote.Map)));
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
    }
}