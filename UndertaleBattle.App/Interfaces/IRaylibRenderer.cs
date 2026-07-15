using UndertaleBattle.Core.Context;

namespace UndertaleBattle.RaylibApp.Interfaces;

public interface IRaylibRenderer
{
    /// <summary>Called once per frame between BeginDrawing/EndDrawing.</summary>
    void Draw(BattleContext context);
}