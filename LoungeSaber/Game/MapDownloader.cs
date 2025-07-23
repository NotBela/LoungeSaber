using System.IO.Compression;
using BeatSaverSharp;
using IPA.Utilities;
using SiraUtil.Logging;
using Zenject;

namespace LoungeSaber.Game;

public class MapDownloader
{
    [Inject] private readonly SiraLog _siraLog = null;
    
    private readonly BeatSaver _beatSaver = new("LoungeSaber", Version.Parse(IPA.Loader.PluginManager.GetPlugin("LoungeSaber").HVersion.ToString()));
    
    public event Action<int, int> OnMapDownloaded;

    public async Task DownloadMaps(string[] mapHashes)
    {
        var mapsDownloaded = 0;
        
        foreach (var mapHash in mapHashes)
        {
            var map = await _beatSaver.BeatmapByHash(mapHash);
            if (map == null)
            {
                _siraLog.Error($"Could not find map {mapHash}!");
                continue;
            }

            var beatmapData = await map.LatestVersion.DownloadZIP();
            
            var zippedBeatmap = new ZipArchive(new MemoryStream(beatmapData ?? throw new Exception("Beatmap data is null!")), ZipArchiveMode.Read);
            zippedBeatmap.ExtractToDirectory(Path.Combine(UnityGame.InstallPath, "Beat Saber_Data", "CustomLevels", $"{map.Name} ({map.ID})"));
            
            mapsDownloaded++;
            OnMapDownloaded?.Invoke(mapsDownloaded, mapHashes.Length);
        }
    }
}