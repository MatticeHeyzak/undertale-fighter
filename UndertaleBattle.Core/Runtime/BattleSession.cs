using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Runtime;

public sealed class BattleSession
{
    public SoulState Player { get; }
    
    public ArenaState Arena { get; }

    public CombatState Combat { get; } = new();

    public DialogueState Dialogue { get; } = new();

    public BattleUiState Ui { get; } = new();

    public List<Item> Inventory { get; } = new();

    public BattleSession(SoulState player, ArenaState arena)
    {
        Player = player ?? throw new ArgumentNullException(nameof(player));
        Arena = arena ?? throw new ArgumentNullException(nameof(arena));
    }

    /// <summary>
    /// Configures dialogue data. The caller returns TextDialogue as its next state.
    /// Session data never performs transition itself.
    /// </summary>
    public void BeginDialogue(string text, BattleStateIdentity continueWith)
    {
        Dialogue.Begin(text, continueWith);
    }
}