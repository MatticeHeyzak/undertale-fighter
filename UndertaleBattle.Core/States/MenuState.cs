using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.States;

/// <summary>
/// Handles the FIGHT, ACT, ITEM, and MERCY command row.
/// </summary>
public sealed class MenuState : IBattleState
{
    public const int OptionCount = 4;

    private const int FightIndex = 0;
    private const int ActIndex = 1;
    private const int ItemIndex = 2;
    private const int MercyIndex = 3;

    public BattleStateIdentity Identity => BattleStateIdentity.Menu;

    public BattleStateIdentity? Enter(BattleSession session)
    {
        session.Ui.CommandMenu.Reset();
        return null;
    }

    public BattleStateIdentity? Update(
        BattleSession session,
        BattleInput input,
        float deltaTime)
    {
        return input.MenuAction switch
        {
            MenuInput.Left => MoveLeft(session),
            MenuInput.Right => MoveRight(session),
            MenuInput.Confirm => ResolveSelection(session),
            _ => null
        };
    }

    public void Exit(BattleSession session)
    {
    }

    private static BattleStateIdentity? MoveLeft(BattleSession session)
    {
        session.Ui.CommandMenu.MoveLeft(OptionCount);
        return null;
    }

    private static BattleStateIdentity? MoveRight(BattleSession session)
    {
        session.Ui.CommandMenu.MoveRight(OptionCount);
        return null;
    }

    private static BattleStateIdentity ResolveSelection(BattleSession session)
    {
        return session.Ui.CommandMenu.SelectedIndex switch
        {
            FightIndex => ResolveFight(session),
            ActIndex => ResolveAct(session),
            ItemIndex => ResolveItem(session),
            MercyIndex => ResolveMercy(session),
            _ => throw new InvalidOperationException(
                $"Unknown menu index '{session.Ui.CommandMenu.SelectedIndex}'.")
        };
    }

    private static BattleStateIdentity ResolveFight(BattleSession session)
    {
        if (session.Combat.CurrentEnemy is not null)
            return BattleStateIdentity.AttackQte;

        session.BeginDialogue(
            "There is nothing to fight.",
            BattleStateIdentity.Menu);

        return BattleStateIdentity.TextDialogue;
    }

    private static BattleStateIdentity ResolveAct(BattleSession session)
    {
        var enemy = session.Combat.CurrentEnemy;

        string text = enemy is null
            ? "There is nothing here to act on."
            : $"* Check\n{enemy.Name} - {enemy.CheckDescription}";

        session.BeginDialogue(text, BattleStateIdentity.EnemyTurn);

        return BattleStateIdentity.TextDialogue;
    }

    private static BattleStateIdentity ResolveItem(BattleSession session)
    {
        if (session.Inventory.Count == 0)
        {
            session.BeginDialogue(
                "You have no items!",
                BattleStateIdentity.Menu);

            return BattleStateIdentity.TextDialogue;
        }

        // Inventory selection is intentionally deferred to a later menu phase.
        var item = session.Inventory[0];
        session.Inventory.RemoveAt(0);

        session.Player.Heal(item.HealAmount);

        session.BeginDialogue(
            item.BuildUseDialogue(),
            BattleStateIdentity.EnemyTurn);

        return BattleStateIdentity.TextDialogue;
    }

    private static BattleStateIdentity ResolveMercy(BattleSession session)
    {
        string enemyName =
            session.Combat.CurrentEnemy?.Name ?? "the enemy";

        session.BeginDialogue(
            $"You spare {enemyName}... but it does not work yet.",
            BattleStateIdentity.EnemyTurn);

        return BattleStateIdentity.TextDialogue;
    }
}