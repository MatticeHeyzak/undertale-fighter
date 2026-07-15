using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.States;

public class MenuState : IBattleState
{
    public BattleStateIdentity Identity => BattleStateIdentity.Menu;
    public const int OptionCount = 4;

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
            case 0:
                context.StateMachine.ChangeState(BattleStateIdentity.EnemyTurn, context);
                break;
            case 3:
                context.BattleOver = true;
                break;
        }
    }
}