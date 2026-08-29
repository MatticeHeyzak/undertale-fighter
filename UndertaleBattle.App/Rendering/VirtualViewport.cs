using System.Numerics;
using Raylib_cs;

namespace UndertaleBattle.Rendering;

/// <summary>
/// Renders the game at its fixed virtual resolution, then presents it at the
/// current physical window size while preserving aspect ratio
/// </summary>
public sealed class VirtualViewport : IDisposable
{
    private RenderTexture2D _renderTarget;
    private bool _initialized;
    private bool _disposed;

    public void Initialize()
    {
        ThrowIfDisposed();
        
        if (_initialized)
            throw new InvalidOperationException("The viewport has already been initialized.");
        
        _renderTarget = Raylib.LoadRenderTexture(
            Settings.VirtualWidth,
            Settings.VirtualHeight);
        
        Raylib.SetTextureFilter(
            _renderTarget.Texture,
            TextureFilter.Point);
        
        _initialized = true;
    }

    public void BeginScene()
    {
        EnsureInitialized();
        
        Raylib.BeginTextureMode(_renderTarget);
        Raylib.ClearBackground(Color.Black);
    }
    
    public void EndScene()
    {
        EnsureInitialized();
        Raylib.EndTextureMode();
    }

    public void Present()
    {
        EnsureInitialized();
        
        var scale = MathF.Min(
            Raylib.GetScreenWidth() / (float)Settings.VirtualWidth,
            Raylib.GetScreenHeight() / (float)Settings.VirtualHeight);
        
        var destinationWidth = Settings.VirtualWidth * scale;
        var destinationHeight = Settings.VirtualHeight * scale;
        
        var destinationX =
            (Raylib.GetScreenWidth() - destinationWidth) / 2f;
        
        var destinationY =
            (Raylib.GetScreenHeight() - destinationHeight) / 2f;
        
        var source = new Rectangle(
            0f,
            0f,
            Settings.VirtualWidth,
            -Settings.VirtualHeight);

        var destination = new Rectangle(
            destinationX,
            destinationY,
            destinationWidth,
            destinationHeight);
        
        Raylib.DrawTexturePro(
            _renderTarget.Texture,
            source,
            destination,
            Vector2.Zero,
            0f,
            Color.White);
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        
        if (_initialized)
            Raylib.UnloadRenderTexture(_renderTarget);

        _disposed = true;
    }

    private void EnsureInitialized()
    {
        ThrowIfDisposed();

        if (!_initialized)
        {
            throw new InvalidOperationException("Initialize must be called after Raylib.InitWindow.");
        }
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }
}