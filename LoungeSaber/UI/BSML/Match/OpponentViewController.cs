using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using LoungeSaber.Interfaces;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Server;
using Zenject;

namespace LoungeSaber.UI.BSML.Match;

[ViewDefinition("LoungeSaber.UI.BSML.Match.OpponentView.bsml")]
public class OpponentViewController : BSMLAutomaticViewController
{
    [UIValue("opponentText")] private string OpponentText { get; set; }
    
    public void PopulateData(Models.UserInfo.UserInfo opponent)
    {
        OpponentText = $"{opponent.GetFormattedUserName()} - {opponent.Mmr} MMR";
        NotifyPropertyChanged(nameof(OpponentText));
    }
}