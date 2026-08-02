using UndertaleBattle;
using UndertaleBattle.Assets;
using UndertaleBattle.Input;
using UndertaleBattle.Rendering;
using UndertaleBattle.Renderers;
using UndertaleBattle.Renderers.States;

var assets = new AssetStore();
var sprites = new SpriteStore(assets);

var renderer = new StateRendererFactory(
    sharedRenderer: new SharedRenderer(sprites),
    renderers:
    [
        new MenuRenderer(sprites),
        new PlayerDodgingRenderer(sprites),
        new DialogueRenderer(assets),
        new AttackQteRenderer(sprites)
    ]);

new GameEngine(
    new BattleFactory(),
    renderer,
    assets,
    new RaylibInputState()).Run();