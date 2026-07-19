using Raylib_cs;

namespace UndertaleBattle.Assets;

/// <summary>
/// Loads and owns all Raylib assets.
/// Call <see cref="LoadAll"/> once on startup (after InitWindow)
/// Call <see cref="Dispose"/> on shutdown (before CloseWIndow)
/// </summary>
public sealed class AssetStore : IAssetStore, IDisposable
{
    private readonly Dictionary<string, Texture2D> _textures = new();
    private readonly Dictionary<string, Font> _fonts = new();

    private bool _disposed;

    public void LoadAll()
    {
        foreach (var (key, path) in AssetManifest.Textures)
            LoadTexture(key, path);
        
        foreach (var (key, path) in AssetManifest.Fonts)
            LoadFont(key, path);
    }

    private void LoadTexture(string key, string path)
    {
        if (!File.Exists(path))
        {
            Console.Error.WriteLine($"[AssetStore] WARNING: texture not found: {path} (key: {key})");
            return;
        }
        _textures[key] = Raylib.LoadTexture(path);
    }
    
    private void LoadFont(string key, string path)
    {
        if (!File.Exists(path))
        {
            Console.Error.WriteLine($"[AssetStore] WARNING: texture not found: {path} (key: {key})");
            return;
        }
        _fonts[key] = Raylib.LoadFont(path);
    }
    
    // --- IAssetStore ---
    
    public Texture2D GetTexture(string key)
    {
        if (_textures.TryGetValue(key, out var t)) return t;
        throw new KeyNotFoundException($"[AssetStore] Texture not loaded: '{key}'");
    }

    public Font GetFont(string key)
    {
        if (_fonts.TryGetValue(key, out var f)) return f;
        throw new KeyNotFoundException($"[AssetStore] Font not loaded: '{key}'");
    }
    
    public bool TryGetTexture(string key, out Texture2D texture) => _textures.TryGetValue(key, out texture);
    public bool TryGetFont(string key, out Font font)             => _fonts.TryGetValue(key, out font);
    
    // --- IDisposable ---

    public void Dispose()
    {
        if (_disposed) return;
        foreach (var t in _textures.Values) Raylib.UnloadTexture(t);
        foreach (var t in _fonts.Values) Raylib.UnloadFont(t);
        _disposed = true;
    }
}