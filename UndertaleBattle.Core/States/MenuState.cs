using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

public class MenuState : IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.Menu;
    public const int OptionCount = 4;

    private const int FightIndex = 0;
    private const int ActIndex   = 1;
    private const int ItemIndex  = 2;
    private const int MercyIndex = 3;

    private const int FightDamage = 10;

    public void Enter(BattleContext context)
    {
        context.SelectedMenuIndex = 0;
    }

    public void Update(BattleContext context, float deltaTime)
    {
        switch (context.PendingMenuInput)
        {
            case MenuInput.Left:
                context.SelectedMenuIndex = Math.Max(0, context.SelectedMenuIndex - 1);
                break;
            case MenuInput.Right:
                context.SelectedMenuIndex = Math.Min(OptionCount - 1, context.SelectedMenuIndex + 1);
                break;
            case MenuInput.Confirm:
                HandleConfirm(context);
                break;
        }

        context.PendingMenuInput = MenuInput.None;
    }
    
    public void Exit(BattleContext context) { }
    
    private static void HandleConfirm(BattleContext context)
    {
        switch (context.SelectedMenuIndex)
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
        }
    }

    private static void ResolveFight(BattleContext context)
    {
        var enemy = context.CurrentEnemy;
        if (enemy is null)
            return;

        enemy.TakeDamage(FightDamage);

        string dialog = enemy.IsDead
            ? $"You attack! {enemy.Name} takes {FightDamage} damage and collapses!"
            : $"You attack! {enemy.Name} takes {FightDamage} damage.";

        // TODO: once win/lose flow exists, route defeated enemies to a Victory state
        // instead of straight into another EnemyTurn.
        context.ShowDialogue(dialog, BattleStateIdentity.EnemyTurn);
    }

    private static void ResolveAct(BattleContext context)
    {
        var enemy = context.CurrentEnemy;

        // TODO: replace with a sub-menu of named acts once more than "Check" exists.
        string dialog = enemy is null
            ? "There's nothing here to act on."
            : $"* Check\n{enemy.Name} - {enemy.CheckDescription}";

        context.ShowDialogue(dialog, BattleStateIdentity.EnemyTurn);
    }

    private static void ResolveItem(BattleContext context)
    {
        // TODO: replace with a sub-menu once the inventory has more than one slot.
        if (context.Inventory.Count == 0)
        {
            context.ShowDialogue("You have no items!", BattleStateIdentity.Menu);
            return;
        }

        var item = context.Inventory[0];
        context.Inventory.RemoveAt(0);

        context.PlayerSoul.Heal(item.HealAmount);

        context.ShowDialogue(item.BuildUseDialogue(), BattleStateIdentity.EnemyTurn);
    }

    private static void ResolveMercy(BattleContext context)
    {
        context.ShowDialogue(
            $"You spare {context.CurrentEnemy?.Name ?? "the enemy"}... but it doesn't work yet.",
            BattleStateIdentity.EnemyTurn);
    }
}