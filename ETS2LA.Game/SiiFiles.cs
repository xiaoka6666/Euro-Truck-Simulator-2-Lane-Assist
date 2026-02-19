using TruckLib;
using TruckLib.Sii;
using ETS2LA.Logging;

namespace ETS2LA.Game.SiiFiles;

public class SiiFileHandler
{
    private static readonly Lazy<SiiFileHandler> _instance = new(() => new SiiFileHandler());
    public static SiiFileHandler Current => _instance.Value;

    Dictionary<string, SiiFile> _cache = new Dictionary<string, SiiFile>();
    IFileSystem? _fs;

    public void SetFileSystem(IFileSystem fs)
    {
        _fs = fs;
    }

    public SiiFile? GetSiiFile(string path)
    {
        if (_cache.ContainsKey(path))
        {
            return _cache[path];
        }

        try
        {
            var sii = SiiFile.Open(path, _fs);
            _cache[path] = sii;
            return sii;
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to load SII file at {path}: {ex.Message}");
            return null;
        }
    }
}