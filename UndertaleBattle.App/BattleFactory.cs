using System.Numerics;
using UndertaleBattle.Core;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;
using UndertaleBattle.Core.Patterns;
using UndertaleBattle.Core.Runtime;
using UndertaleBattle.Core.States;
using UndertaleBattle.Core.Systems;

namespace UndertaleBattle;

public interface IBattleFactory
{
    BattleSimulation Create();
}

/// <summary>
/// Creates a complete, fresh battle runtime. Restarting creates new runtime state
/// rather than attempting to reset stateful systems in place.
/// </summary>
public sealed class BattleFactory : IBattleFactory
{
    public BattleSimulation Create()
    {
        var arenaShape = new BattleArena(
            position: new Vector2(30f, 280f),
            width: Settings.ScreenWidth - 70f,
            height: 200f);

        var player = new SoulState(
            maxHealth: 20,
            startPosition: arenaShape.Position + new Vector2(200f, 150f));

        var enemy = new Enemy("Froggit", maxHealth: 50)
        {
            CheckDescription = "ATK 4 DEF 4. Doesn't like fighting much either."
        };

        var session = new BattleSession(
            player,
            new ArenaState(arenaShape),
            enemy,
            inventory:
            [
                new Item
                {
                    Name = "Snack",
                    HealAmount = 5,
                    UseDialogueTemplate = "You ate the {0}. Healed 5 HP!"
                }
            ]);

        var soulSystem = new SoulSystem();
        var projectileSystem = new ProjectileSystem();
        var collisionSystem = new CollisionSystem();

        var machine = new BattleStateMachine();
        machine.RegisterState(new MenuState());
        machine.RegisterState(new TextDialogueState());
        IAttackSelector attackSelector = new FixedAttackSelector(
            () => new BarragePattern(
                bulletCount: 5,
                speed: 180f,
                damage: 4,
                duration: 6f));

        machine.RegisterState(new EnemyTurnState(attackSelector));
        machine.RegisterState(new PlayerDodgingState(
            soulSystem,
            projectileSystem,
            collisionSystem));
        machine.RegisterState(new AttackQteState());

        return new BattleSimulation(
            session,
            machine,
            new ArenaSystem());
    }
}