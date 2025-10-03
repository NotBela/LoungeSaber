using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using Zenject;

namespace LoungeSaber.UI.BSML.Match;

[ViewDefinition("LoungeSaber.UI.BSML.Match.OpponentView.bsml")]
public class OpponentViewController : BSMLAutomaticViewController, IInitializable, IDisposable
{
    [Inject] private readonly IServerListener _serverListener = null;
    
    [UIValue("opponentText")] private string OpponentText { get; set; }
    
    public void PopulateData(MatchCreatedPacket packet)
    {
        OpponentText = $"{packet.Opponent.GetFormattedUserName()} - {packet.Opponent.Mmr} MMR";
        NotifyPropertyChanged(nameof(OpponentText));
    }

    public void Initialize()
    {
        _serverListener.OnMatchCreated += PopulateData;
    }

    public void Dispose()
    {
        _serverListener.OnMatchCreated -= PopulateData;
    }
}