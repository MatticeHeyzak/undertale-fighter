using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Handles the FIGHT, ACT, ITEM, and MERCY command row.
/// </summary>
public sealed class MenuState : IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.Menu;

    public const int OptionCount = 4;

    private const int FightIndex = 0;
    private const int ActIndex = 1;
    private const int ItemIndex = 2;
    private const int MercyIndex = 3;

    public void Enter(BattleContext context)
    {
        context.Menu.Reset();
        context.ClearTransientInput();
    }

    public void Update(BattleContext context, float deltaTime)
    {
        switch (context.PendingMenuInput)
        {
            case MenuInput.Left:
                context.Menu.MoveLeft(OptionCount);
                break;

            case MenuInput.Right:
                context.Menu.MoveRight(OptionCount);
                break;

            case MenuInput.Confirm:
                HandleConfirm(context);
                break;
        }

        context.ClearTransientInput();
    }

    public void Exit(BattleContext context)
    {
    }

    private static void HandleConfirm(BattleContext context)
    {
        switch (context.Menu.SelectedIndex)
        {
            case FightIndex:
                ResolveFight(context);
                break;

            case ActIndex:
                ResolveAct(context);
                break;

            case ItemIndex:
                ResolveItem(context);
                break;

            case MercyIndex:
                ResolveMercy(context);
                break;

            default:
                throw new InvalidOperationException(
                    $"Unknown menu index '{context.Menu.SelectedIndex}'.");
        }
    }

    private static void ResolveFight(BattleContext context)
    {
        if (context.CurrentEnemy is null)
        {
            context.ShowDialogue(
                "There is nothing to fight.",
                BattleStateIdentity.Menu);
            return;
        }

        context.StateMachine.ChangeState(BattleStateIdentity.AttackQte, context);
    }

    private static void ResolveAct(BattleContext context)
    {
        var enemy = context.CurrentEnemy;

        string dialogue = enemy is null
            ? "There is nothing here to act on."
            : $"* Check\n{enemy.Name} - {enemy.CheckDescription}";

        context.ShowDialogue(dialogue, BattleStateIdentity.EnemyTurn);
    }

    private static void ResolveItem(BattleContext context)
    {
        if (context.Inventory.Count == 0)
        {
            context.ShowDialogue(
                "You have no items!",
                BattleStateIdentity.Menu);
            return;
        }

        // Phase 1 retains the existing first-item behavior.
        // A proper inventory-selection state belongs in a later phase.
        var item = context.Inventory[0];
        context.Inventory.RemoveAt(0);

        context.PlayerSoul.Heal(item.HealAmount);

        context.ShowDialogue(
            item.BuildUseDialogue(),
            BattleStateIdentity.EnemyTurn);
    }

    private static void ResolveMercy(BattleContext context)
    {
        string enemyName = context.CurrentEnemy?.Name ?? "the enemy";

        context.ShowDialogue(
            $"You spare {enemyName}... but it does not work yet.",
            BattleStateIdentity.EnemyTurn);
    }
}
