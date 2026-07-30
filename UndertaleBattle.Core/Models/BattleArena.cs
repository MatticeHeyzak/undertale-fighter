using System.Numerics;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.Models;

/// <summary>
/// Rectangular arena that can move and resize over time <see cref="AnimateTo"/>
/// For non-rectangular arenas, implement <see cref="IArenaShape"/> directly instead.
/// </summary>
public sealed class BattleArena : IArenaShape
{
    public Vector2 Position { get; private set; }

    public float Width { get; private set; }

    public float Height { get; private set; }

    public float Left => Position.X;

    public float Right => Position.X + Width;

    public float Top => Position.Y;

    public float Bottom => Position.Y + Height;

    public Vector2 Center =>
        Position + new Vector2(Width / 2f, Height / 2f);

    private Vector2 _fromPosition;
    private Vector2 _toPosition;
    private float _fromWidth;
    private float _toWidth;
    private float _fromHeight;
    private float _toHeight;
    private float _animationElapsed;
    private float _animationDuration;
    private bool _isAnimating;

    public BattleArena(Vector2 position, float width, float height)
    {
        if (width <= 0f)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (height <= 0f)
            throw new ArgumentOutOfRangeException(nameof(height));

        Position = position;
        Width = width;
        Height = height;
    }

    public Vector2 Clamp(Vector2 point)
    {
        return new Vector2(
            Math.Clamp(point.X, Left, Right),
            Math.Clamp(point.Y, Top, Bottom));
    }

    public bool Contains(Vector2 point)
    {
        return point.X >= Left &&
               point.X <= Right &&
               point.Y >= Top &&
               point.Y <= Bottom;
    }

    public void AnimateTo(
        Vector2 position,
        float width,
        float height,
        float duration)
    {
        if (width <= 0f)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (height <= 0f)
            throw new ArgumentOutOfRangeException(nameof(height));

        if (duration <= 0f)
        {
            Position = position;
            Width = width;
            Height = height;
            _isAnimating = false;
            return;
        }

        _fromPosition = Position;
        _toPosition = position;

        _fromWidth = Width;
        _toWidth = width;

        _fromHeight = Height;
        _toHeight = height;

        _animationElapsed = 0f;
        _animationDuration = duration;
        _isAnimating = true;
    }

    public void Update(float deltaTime)
    {
        if (!_isAnimating)
            return;

        _animationElapsed += deltaTime;

        float progress = Math.Clamp(
            _animationElapsed / _animationDuration,
            0f,
            1f);

        Position = Vector2.Lerp(
            _fromPosition,
            _toPosition,
            progress);

        Width = float.Lerp(_fromWidth, _toWidth, progress);
        Height = float.Lerp(_fromHeight, _toHeight, progress);

        if (progress >= 1f)
            _isAnimating = false;
    }
}