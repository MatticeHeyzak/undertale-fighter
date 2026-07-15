using System.Numerics;
using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Context;

public class BattleContext
{
    public HeartSoul PlayerSoul { get; }
    
    public BattleStateIdentity CurrentState { get; internal set; }
    
    public int SelectedMenuIndex { get; set; }
    
    public bool BattleOver { get; set; }

    public string CurrentDialog { get; set; } = string.Empty;
    
    public MenuInput PendingMenuInput { get; set; }
    public int VisibleDialogCharCount { get; set; }
    public Vector2 MovementInput { get; set; } 

    public BattleArena Arena { get; }
    public List<Bullet> Bullets { get; } = new();
    public Enemy? CurrentEnemy { get; set; }
    
    public IBattleStateMachine StateMachine { get; }

    public BattleContext(IBattleStateMachine stateMachine, HeartSoul playerSoul, BattleArena arena)
    {
        StateMachine = stateMachine;
        PlayerSoul = playerSoul;
        Arena = arena;
    }
}