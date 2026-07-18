using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Interfaces;

namespace UndertaleBattle.Renderers;

/// <summary>
/// Routes Draw() to the correct state-specific renderer.
/// Add a new IStateRenderer, zero changes to existing renderers.
/// </summary>
public sealed class StateRendererFactory : IRaylibRenderer
{
    private readonly Dictionary<BattleStateIdentity, IStateRenderer> _renderers = new();
    private readonly IRaylibRenderer _sharedRenderer; // HUD, arena etc.

    public StateRendererFactory(IRaylibRenderer sharedRenderer, IEnumerable<IStateRenderer> renderers)
    {
        _sharedRenderer = sharedRenderer;
        foreach (var r in renderers)
            _renderers[r.TargetState] = r;
    }

    public void Draw(BattleContext context)
    {
        _sharedRenderer.Draw(context);

        if (_renderers.TryGetValue(context.CurrentState, out var stateRenderer))
            stateRenderer.Draw(context);
    }
}