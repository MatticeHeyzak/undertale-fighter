using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Interfaces;

public interface IBulletMotion
{
    void Update(Bullet bullet, float deltaTime);
}