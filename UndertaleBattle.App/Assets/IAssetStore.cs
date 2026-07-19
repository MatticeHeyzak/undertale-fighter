using Raylib_cs;

namespace UndertaleBattle.Assets;

/// <summary>
/// Provides loaded assets by key.
/// </summary>
public interface IAssetStore
{
    /// <summary>
    /// Returns the texture registered under <paramref name="key"/>.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Key was not loaded.</exception>
    Texture2D GetTexture(string key);

    /// <summary>
    /// Returns the font registered under <paramref name="key"/>
    /// </summary>
    /// <exception cref="KeyNotFoundException">Key was not loaded.</exception>
    Font GetFont(string key);
    
    bool TryGetTexture(string key, out Texture2D texture);
    bool TryGetFont(string key, out Font font);
}