using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.Runtime;

/// <summary>
/// Runtime arena state. Geometry/control remain encapsulated by <see cref="Shape"/>
/// </summary>
public sealed class ArenaState
{
    public IArenaShape Shape { get; }

    public ArenaState(IArenaShape shape)
    {
        Shape = shape ?? throw new ArgumentNullException(nameof(shape));
    }
}