using Newtonsoft.Json;

namespace LoungeSaber.Models.Packets.ServerPackets.Match;

public record Score
{
    public Score(int Points, float RelativeScore, bool ProMode, int Misses, bool FullCombo)
    {
        this.Points = Points;
        this.RelativeScore = RelativeScore;
        this.ProMode = ProMode;
        this.Misses = Misses;
        this.FullCombo = FullCombo;
    }

    public static Score Empty => new Score(0, 0f, false, 0, true);
    public int Points { get; }
    public float RelativeScore { get; }
    public bool ProMode { get; }
    public int Misses { get; }
    public bool FullCombo { get; }
}