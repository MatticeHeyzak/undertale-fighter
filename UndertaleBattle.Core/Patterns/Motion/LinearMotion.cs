using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Models;

namespace UndertaleBattle.Core.Patterns.Motion;

public class LinearMotion : IBulletMotion
{
    public void Update(Bullet bullet, float deltaTime) => bullet.Position += bullet.Velocity * deltaTime;
}