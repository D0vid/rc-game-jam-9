using System.Collections.Generic;
using System.Linq;
using Battlers;
using Grid;
using UI.BattleUI;
using UnityEngine;
using Utils.Channels;

namespace StateManagement
{
    public class SkillCastManager : MonoBehaviour
    {
        [SerializeField] private BattleChannel battleChannel;

        private DamageCalculator _damageCalculator;

        private void Awake()
        {
            _damageCalculator = new DamageCalculator();
        }

        private void ResolveSkillCast(BattlerInstance currentBattler, Skill skill, List<Node> skillShape, IEnumerable<BattlerInstance> aliveBattlers)
        {
            var hitBattlers = DetermineHitBattlers(skillShape, aliveBattlers);
            foreach (var hitBattler in hitBattlers)
            {
                var damage = _damageCalculator.CalculateDamage(currentBattler, hitBattler, skill);
                hitBattler.TakeDamage(damage);
            }
        }

        private IEnumerable<BattlerInstance> DetermineHitBattlers(IEnumerable<Node> currentShape, IEnumerable<BattlerInstance> aliveBattlers)
        {
            return aliveBattlers
                .Where(b => currentShape
                    .Select(s => s.WorldPosition)
                    .ToList()
                    .Contains(b.Position))
                .ToList();
        }

        private void OnEnable()
        {
            battleChannel.skillCastEvent += ResolveSkillCast;
        }
    }
}