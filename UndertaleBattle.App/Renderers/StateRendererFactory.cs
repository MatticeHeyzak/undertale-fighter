using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Runtime;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle.Renderers;

/// <summary>
/// Routes Draw() to the correct state-specific renderer.
/// Add a new IStateRenderer, zero changes to existing renderers.
/// </summary>
public sealed class StateRendererFactory : IRaylibRenderer
{
    private readonly Dictionary<BattleStateIdentity, IStateRenderer> _renderers = new();
    private readonly IRaylibRenderer _sharedRenderer;

    public StateRendererFactory(
        IRaylibRenderer sharedRenderer,
        IEnumerable<IStateRenderer> renderers)
    {
        _sharedRenderer =
            sharedRenderer ?? throw new ArgumentNullException(nameof(sharedRenderer));

        foreach (var renderer in renderers)
        {
            if (!_renderers.TryAdd(renderer.TargetState, renderer))
            {
                throw new InvalidOperationException(
                    $"A renderer is already registered for '{renderer.TargetState}'.");
            }
        }
    }

    public void Draw(
        BattleSession session,
        BattleStateIdentity currentState)
    {
        _sharedRenderer.Draw(session, currentState);

        if (_renderers.TryGetValue(currentState, out var renderer))
            renderer.Draw(session);
    }
}