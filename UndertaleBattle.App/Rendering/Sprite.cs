using System.Numerics;
using Raylib_cs;

namespace UndertaleBattle.Rendering;

/// <summary>
/// Describes how a texture should be drawn - source region, pivot and scale.
/// The renderer uses this;
/// </summary>
public sealed class Sprite
{
    /// <summary>
    /// The full texture this sprite samples from.
    /// </summary>
    public Texture2D Texture { get; }
    
    /// <summary>
    /// Region of the texture to draw. For a full texture (0, 0, width, height).
    /// For a spritesheet frame: the frame's rect
    /// </summary>
    public Rectangle SourceRect { get; set; }
    
    /// <summary>
    /// Draw origin relative to SourceRect, in pixels.
    /// (0, 0) = top-left. (width/2, height/2) = centered.
    /// </summary>
    public Vector2 Origin { get; set; }

    /// <summary>
    /// Uniform draw scale.
    /// </summary>
    public float Scale { get; set; }

    public Sprite(Texture2D texture, Rectangle? sourceRect = null, Vector2? origin = null, float scale = 1f)
    {
        Texture = texture;
        SourceRect = sourceRect ?? new Rectangle(0, 0, texture.Width, texture.Height);
        Origin = origin ?? new Vector2(texture.Width / 2f, texture.Height / 2f);
        Scale = scale;
    }
    
    /// <summary>
    /// Destination rect at a given world position.
    /// </summary>
    public Rectangle DestRect(Vector2 position) => new(
        position.X - Origin.X * Scale,
        position.Y - Origin.Y * Scale,
        SourceRect.Width  * Scale,
        SourceRect.Height * Scale
    );
}
