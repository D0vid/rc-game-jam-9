using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battlers;
using DG.Tweening;
using Grid;
using TMPro;
using UnityEngine;
using Utils.Channels;

namespace StateManagement
{
    public class SkillCastManager : MonoBehaviour
    {
        [SerializeField] private BattleChannel battleChannel;
        [SerializeField] private GameObject textEffectPrefab;

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
                if (skill.damage != 0)
                {
                    var damage = _damageCalculator.CalculateDamage(currentBattler, hitBattler, skill);
                    hitBattler.CurrentHp -= damage;
                }
                ApplyEffects(hitBattler, skill.effects);
            }
        }

        private void ApplyEffects(BattlerInstance target, Effect[] effects)
        {
            foreach (var effect in effects)
            {
                ApplyEffect(target, effect);
            }
        }

        private void ApplyEffect(BattlerInstance target, Effect effect)
        {
            int value = (int)effect.value;

            switch (effect.stat)
            {
                case Stat.Health:
                    target.CurrentHp += value;
                    break;
                case Stat.MovementPoints:
                    target.CurrentMp += value;
                    break;
                case Stat.PowerPoints:
                    target.CurrentPp += value;
                    break;
                case Stat.Range:
                    target.CurrentRange += value;
                    break;
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