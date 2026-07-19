using UndertaleBattle.Assets;

namespace UndertaleBattle.Rendering;

/// <summary>
/// Constructs ready-to-draw sprites instances from IAssetStore textures.
/// </summary>
public sealed class SpriteStore
{
    private readonly IAssetStore _assets;
    private readonly Dictionary<string, Sprite> _cache = new();

    public SpriteStore(IAssetStore assets)
    {
        _assets = assets;
    }

    /// <summary>
    /// Returns a cached Sprite for the given key.
    /// </summary>
    public Sprite? Get(string key)
    {
        if (_cache.TryGetValue(key, out var cached)) return cached;

        if (!_assets.TryGetTexture(key, out var tex)) return null;
        
        var sprite = BuildSprite(key, tex);
        _cache[key] = sprite;
        return sprite;
    }
    
    private static Sprite BuildSprite(string key, Raylib_cs.Texture2D tex)
    {
        if (key.StartsWith("ui/button"))
            return new Sprite(tex, scale: 1.5f); // centered pivot

        if (key.StartsWith("soul/"))
            return new Sprite(tex, scale: 0.03f); // centered, natural size

        if (key.StartsWith("enemies/"))
            return new Sprite(tex); // centered

        if (key.StartsWith("bullets/"))
            return new Sprite(tex); // centered

        return new Sprite(tex); // sensible default
    }
}