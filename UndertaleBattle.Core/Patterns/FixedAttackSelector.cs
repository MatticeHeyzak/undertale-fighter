using UndertaleBattle.Core.Interfaces;
using UndertaleBattle.Core.Runtime;

namespace UndertaleBattle.Core.Patterns;

/// <summary>
/// Creates a fresh instance of one configured attack every enemy turn.
/// </summary>
public sealed class FixedAttackSelector : IAttackSelector
{
    private readonly Func<IAttackPattern> _createAttack;

    public FixedAttackSelector(Func<IAttackPattern> createAttack)
    {
        _createAttack = createAttack ?? throw new ArgumentNullException(nameof(createAttack));
    }

    public IAttackPattern CreateNextAttack(BattleSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        return _createAttack()
               ?? throw new InvalidOperationException(
                   "The configured attack factory returned null.");
    }
}