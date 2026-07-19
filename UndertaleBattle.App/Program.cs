using System.Numerics;
using UndertaleBattle;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Models;
using UndertaleBattle.Core.Patterns;
using UndertaleBattle.Core.States;
using UndertaleBattle.Rendering;
using UndertaleBattle.Renderers;
using UndertaleBattle.Renderers.States;

var arena   = new BattleArena(new Vector2(30, 280), Settings.ScreenWidth - 70, 200);
var soul    = new HeartSoul(maxHealth: 20, startPosition: arena.Position + new Vector2(200, 150));
var enemy   = new Enemy("Froggit", maxHealth: 50);
var machine = new BattleStateMachine();
var context = new BattleContext(machine, soul, arena) { CurrentEnemy = enemy };

machine.RegisterState(new MenuState());
machine.RegisterState(new TextDialogueState(nextState: BattleStateIdentity.PlayerDodging));
machine.RegisterState(new EnemyTurnState(new BarragePattern()));
machine.RegisterState(new PlayerDodgingState(phaseDuration: 6f));

var assets  = new AssetStore();
var sprites = new SpriteStore(assets);   // built on top of AssetStore

var renderer = new StateRendererFactory(
    sharedRenderer: new SharedRenderer(assets),
    renderers: [
        new MenuRenderer(sprites),
        new PlayerDodgingRenderer(sprites),
    ]
);

machine.ChangeState(BattleStateIdentity.Menu, context);
new GameEngine(context, machine, renderer, assets).Run();