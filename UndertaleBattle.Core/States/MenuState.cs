using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Input;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;
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
        if (session.IsComplete)
            return null;

        if (input.ConfirmPressed)
            return ResolveSelection(session);

        if (input.LeftPressed)
            return MoveLeft(session);

        if (input.RightPressed)
            return MoveRight(session);

        return null;
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
            FightIndex => BattleStateIdentity.AttackQte,
            ActIndex => ResolveAct(session),
            ItemIndex => ResolveItem(session),
            MercyIndex => ResolveMercy(session),
            _ => throw new InvalidOperationException(
                $"Unknown menu index '{session.Ui.CommandMenu.SelectedIndex}'.")
        };
    }

    private static BattleStateIdentity ResolveAct(BattleSession session)
    {
        var enemy = session.Combat.CurrentEnemy;

        session.BeginDialogue(
            $"* Check\n{enemy.Name} - {enemy.CheckDescription}",
            BattleStateIdentity.EnemyTurn);

        return BattleStateIdentity.TextDialogue;
    }

    private static BattleStateIdentity ResolveItem(BattleSession session)
    {
        if (!session.TryConsumeFirstItem(out Item? item))
        {
            session.BeginDialogue(
                "You have no items!",
                BattleStateIdentity.Menu);

            return BattleStateIdentity.TextDialogue;
        }

        session.Player.Heal(item.HealAmount);

        session.BeginDialogue(
            item.BuildUseDialogue(),
            BattleStateIdentity.EnemyTurn);

        return BattleStateIdentity.TextDialogue;
    }

    private static BattleStateIdentity ResolveMercy(BattleSession session)
    {
        session.BeginDialogue(
            $"You spare {session.Combat.CurrentEnemy.Name}... but it does not work yet.",
            BattleStateIdentity.EnemyTurn);

        return BattleStateIdentity.TextDialogue;
    }
}