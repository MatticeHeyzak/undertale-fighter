namespace UndertaleBattle.Core.Input;

/// <summary>
/// Application-layer input adapter.
/// </summary>
public interface IBattleInputSource
{
    BattleInput Poll();
}