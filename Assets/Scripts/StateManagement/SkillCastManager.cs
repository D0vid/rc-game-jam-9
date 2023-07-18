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
                var damage = _damageCalculator.CalculateDamage(currentBattler, hitBattler, skill);
                StartCoroutine(SpawnDamageText(hitBattler, damage));
                hitBattler.TakeDamage(damage);
            }
        }

        private IEnumerator SpawnDamageText(BattlerInstance hitBattler, int damage)
        {
            var instance = Instantiate(textEffectPrefab, hitBattler.transform);
            instance.transform.position += new Vector3(0, 0.5f, 0);
            yield return StartCoroutine(PlayEffectTextAnimation(instance.GetComponent<TextMeshPro>(), damage));
            Destroy(instance);
        }
        
        private IEnumerator PlayEffectTextAnimation(TextMeshPro textMesh, int damage)
        {
            yield return StartCoroutine(TextPopupEffect(textMesh, damage, 0.5f));
        }
        
        private IEnumerator TextPopupEffect(TextMeshPro textMesh, int damage, float duration)
        {

            Color color = Color.red;
            textMesh.text = $"-{damage}";
            textMesh.color = color;

            Vector3 targetPosition = new Vector3(0, 0.75f, 0);

            textMesh.alpha = 0;

            Sequence seq = DOTween.Sequence();
            seq.Append(textMesh.transform.DOLocalMove(targetPosition, duration));
            seq.Join(textMesh.DOFade(1, duration / 2));
            seq.Append(textMesh.DOFade(0, duration / 2));

            yield return seq.WaitForCompletion();
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