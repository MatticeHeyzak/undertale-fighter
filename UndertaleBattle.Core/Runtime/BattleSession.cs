using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Runtime;

public sealed class BattleSession
{
    private readonly List<Item> _inventory = new();

    public SoulState Player { get; }

    public ArenaState Arena { get; }

    public CombatState Combat { get; }

    public DialogueState Dialogue { get; } = new();

    public BattleUiState Ui { get; } = new();

    public IReadOnlyList<Item> Inventory => _inventory;

    public BattleOutcome Outcome { get; private set; } = BattleOutcome.InProgress;

    public bool IsComplete => Outcome != BattleOutcome.InProgress;

    public BattleSession(
        SoulState player,
        ArenaState arena,
        Enemy enemy,
        IEnumerable<Item>? inventory = null)
    {
        Player = player ?? throw new ArgumentNullException(nameof(player));
        Arena = arena ?? throw new ArgumentNullException(nameof(arena));
        Combat = new CombatState(enemy);

        if (inventory is null)
            return;

        foreach (Item item in inventory)
            AddItem(item);
    }

    public void AddItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _inventory.Add(item);
    }

    public bool TryConsumeFirstItem(out Item? item)
    {
        if (_inventory.Count == 0)
        {
            item = null;
            return false;
        }

        item = _inventory[0];
        _inventory.RemoveAt(0);
        return true;
    }

    public void Complete(BattleOutcome outcome)
    {
        if (outcome == BattleOutcome.InProgress)
        {
            throw new ArgumentOutOfRangeException(
                nameof(outcome),
                "A completed battle must have a terminal outcome.");
        }

        if (IsComplete)
        {
            throw new InvalidOperationException(
                $"Battle already completed with outcome '{Outcome}'.");
        }

        Outcome = outcome;
        Combat.EndAttack();
    }

    public void BeginDialogue(string text, BattleStateIdentity continueWith)
    {
        Dialogue.Begin(text, continueWith);
    }
}