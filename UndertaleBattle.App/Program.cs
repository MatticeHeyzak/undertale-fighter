using System.Numerics;
using UndertaleBattle;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Models;
using UndertaleBattle.Core.Patterns;
using UndertaleBattle.Core.States;
using UndertaleBattle.Input;
using UndertaleBattle.Rendering;
using UndertaleBattle.Renderers;
using UndertaleBattle.Renderers.States;

var arena = new BattleArena(new Vector2(30, 280), Settings.ScreenWidth - 70, 200);
var soul  = new HeartSoul(maxHealth: 20, startPosition: arena.Position + new Vector2(200, 150));
var enemy = new Enemy("Froggit", maxHealth: 50)
{
    CheckDescription = "ATK 4 DEF 4. Doesn't like fighting much either."
};
var machine = new BattleStateMachine();
var context = new BattleContext(machine, soul, arena) { CurrentEnemy = enemy };
context.Inventory.Add(new Item
{
    Name = "Snack",
    HealAmount = 5,
    UseDialogueTemplate = "You ate the {0}. Healed 5 HP!"
});

machine.RegisterState(new MenuState());
machine.RegisterState(new TextDialogueState());
machine.RegisterState(new EnemyTurnState(new BarragePattern()));
machine.RegisterState(new PlayerDodgingState(phaseDuration: 6f));

var assets  = new AssetStore();
var sprites = new SpriteStore(assets);
var input   = new RaylibInputState();

var renderer = new StateRendererFactory(
    sharedRenderer: new SharedRenderer(assets),
    renderers: [
        new MenuRenderer(sprites),
        new PlayerDodgingRenderer(sprites),
        new DialogueRenderer(assets)
    ]
);

machine.ChangeState(BattleStateIdentity.Menu, context);
new GameEngine(context, machine, renderer, assets, input).Run();