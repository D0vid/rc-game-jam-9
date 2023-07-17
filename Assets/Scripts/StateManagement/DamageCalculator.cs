using System.Linq;
using Battlers;
using UnityEngine;

namespace StateManagement
{
    public class DamageCalculator
    {
        public int CalculateDamage(BattlerInstance currentBattler, BattlerInstance hitBattler, Skill skill)
        {
            if (skill.category == Category.Status || skill.damage == 0)
                return 0;

            var attackModifier = skill.category switch
            {
                Category.Physical => currentBattler.CurrentAtk,
                Category.Special => currentBattler.CurrentSpAtk,
                _ => 0
            };

            var typeModifier = skill.type.GetDamageFactorAgainst(hitBattler.battler.Typing);

            var stabModifier = currentBattler.battler.Typing.Contains(skill.type) ? 1.5f : 1f;

            return Mathf.FloorToInt(skill.damage + skill.damage * (attackModifier / 100) * typeModifier * stabModifier);
        }
    }
}