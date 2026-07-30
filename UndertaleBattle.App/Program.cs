using System.Numerics;
using UndertaleBattle;
using UndertaleBattle.Assets;
using UndertaleBattle.Core;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Models;
using UndertaleBattle.Core.Patterns;
using UndertaleBattle.Core.Runtime;
using UndertaleBattle.Core.States;
using UndertaleBattle.Core.Systems;
using UndertaleBattle.Input;
using UndertaleBattle.Rendering;
using UndertaleBattle.Renderers;
using UndertaleBattle.Renderers.States;

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
    new ArenaState(arenaShape));

session.Combat.CurrentEnemy = enemy;

session.Inventory.Add(new Item
{
    Name = "Snack",
    HealAmount = 5,
    UseDialogueTemplate = "You ate the {0}. Healed 5 HP!"
});

var arenaSystem = new ArenaSystem();
var soulSystem = new SoulSystem();
var projectileSystem = new ProjectileSystem();
var collisionSystem = new CollisionSystem();

var machine = new BattleStateMachine();

machine.RegisterState(new MenuState());
machine.RegisterState(new TextDialogueState());
machine.RegisterState(new EnemyTurnState(new BarragePattern()));

machine.RegisterState(new PlayerDodgingState(
    soulSystem,
    projectileSystem,
    collisionSystem,
    phaseDuration: 6f));

machine.RegisterState(new AttackQteState());

var simulation = new BattleSimulation(
    session,
    machine,
    arenaSystem);

var assets = new AssetStore();
var sprites = new SpriteStore(assets);
var input = new RaylibInputState();

var renderer = new StateRendererFactory(
    sharedRenderer: new SharedRenderer(sprites),
    renderers:
    [
        new MenuRenderer(sprites),
        new PlayerDodgingRenderer(sprites),
        new DialogueRenderer(assets),
        new AttackQteRenderer(sprites)
    ]);

simulation.Start(BattleStateIdentity.Menu);

new GameEngine(
    simulation,
    renderer,
    assets,
    input).Run();