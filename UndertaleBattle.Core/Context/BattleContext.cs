using UndertaleBattle.Core.Enums;
using UndertaleBattle.Core.Interfaces;

namespace UndertaleBattle.Core.Context;

public class BattleContext
{
    public HeartSoul PlayerSoul { get; }
    
    public BattleStateIdentity CurrentState { get; internal set; }
    
    public int SelectedMenuIndex { get; set; }
    
    public bool BattleOver { get; set; }

    public string CurrentDialog { get; set; } = string.Empty;
    
    public IBattleStateMachine StateMachine { get; }

    public BattleContext(IBattleStateMachine stateMachine, HeartSoul playerSoul)
    {
        StateMachine = stateMachine;
        PlayerSoul = playerSoul;
    }
}