namespace UndertaleBattle.Core.Runtime;

/// <summary>
/// Transient UI state which belongs to the active battle session but is not
/// combat-domain state.
/// </summary>
public sealed class BattleUiState
{
    public CommandMenuUiState CommandMenu { get; } = new();

    public AttackQteUiState AttackQte { get; } = new();
}