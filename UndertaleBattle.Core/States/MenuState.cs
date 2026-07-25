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

    public void Enter(BattleContext context) => context.Menu.SelectedIndex = 0;

    public void Update(BattleContext context, float deltaTime)
    {
        switch (context.PendingMenuInput)
        {
            case MenuInput.Left:
                context.Menu.SelectedIndex = (context.Menu.SelectedIndex - 1 + OptionCount) % OptionCount;
                break;
            case MenuInput.Right:
                context.Menu.SelectedIndex = (context.Menu.SelectedIndex + 1) % OptionCount;
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
        }
    }

    private static void ResolveFight(BattleContext context)
    {
        var enemy = context.CurrentEnemy;
        if (enemy is null)
            return;

        context.StateMachine.ChangeState(BattleStateIdentity.AttackQte, context);
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