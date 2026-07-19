using System.Numerics;
using UndertaleBattle.Core.Context;
using UndertaleBattle.Core.Models;
using UndertaleBattle.Core.Patterns;
using UndertaleBattle.Core.States;
using UndertaleBattle.Renderers;
using UndertaleBattle;
using UndertaleBattle.Assets;
using UndertaleBattle.Core.Enums;

// --- Composition Root ---

var arena = new BattleArena(new Vector2(200, 150), 400, 300);
var soul = new HeartSoul(maxHealth: 20, startPosition: arena.Position + new Vector2(200, 150));
var enemy = new Enemy("Froggit", maxHealth: 50);
var machine = new BattleStateMachine();
var context = new BattleContext(machine, soul, arena) { CurrentEnemy = enemy };

// Register states
machine.RegisterState(new MenuState());
machine.RegisterState(new TextDialogueState(nextState: BattleStateIdentity.PlayerDodging));
machine.RegisterState(new EnemyTurnState(new BarragePattern()));
machine.RegisterState(new PlayerDodgingState(phaseDuration: 6f));

// Renderer
var assets = new AssetStore();
var renderer = new ComponentRenderer(assets);

// Start
machine.ChangeState(BattleStateIdentity.Menu, context);
new GameEngine(context, machine, renderer, assets).Run();