using System;
using System.Collections;
using System.Collections.Generic;
using Battlers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Utils.Channels;

namespace StateManagement
{
    public class TextEffectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject textEffectPrefab;
        [SerializeField] private BattleChannel battleChannel;
        [SerializeField] private float textDuration = 0.5f;

        private Dictionary<int, Sequence> _activeSequences;

        private void Awake()
        {
            _activeSequences = new Dictionary<int, Sequence>();
        }

        private void OnStatChanged(BattlerInstance battler, Stat stat, int value)
        {
            StartCoroutine(SpawnTextPopUp(battler, stat, value));
        }
        
        private IEnumerator SpawnTextPopUp(BattlerInstance hitBattler, Stat stat, int value)
        {
            var instance = Instantiate(textEffectPrefab, hitBattler.transform);
            var instanceID = hitBattler.GetInstanceID();
            if (!_activeSequences.ContainsKey(instanceID))
            {
                _activeSequences[instanceID] = null;
            }
            instance.transform.position += new Vector3(0, 0.5f, 0);
            yield return StartCoroutine(PlayEffectTextAnimation(instance.GetComponent<TextMeshPro>(), stat, value, instanceID));
            Destroy(instance);
        }
        
        private IEnumerator PlayEffectTextAnimation(TextMeshPro textMesh, Stat stat, int value, int instanceID)
        {
            Color color = DetermineStatColor(stat);
            textMesh.text = $"{value}";
            textMesh.color = color;

            Vector3 targetPosition = new Vector3(0, 0.75f, 0);

            textMesh.alpha = 0;

            while (_activeSequences[instanceID] != null)
            {
                yield return null;
            }

            Sequence seq = DOTween.Sequence();
            _activeSequences[instanceID] = seq;
            seq.Append(textMesh.transform.DOLocalMove(targetPosition, textDuration));
            seq.Join(textMesh.DOFade(1, textDuration / 2f));
            seq.Append(textMesh.DOFade(0, textDuration / 2f));

            yield return seq.WaitForCompletion();
            _activeSequences[instanceID] = null;
        }

        private Color DetermineStatColor(Stat stat)
        {
            return stat switch
            {
                Stat.Health => Color.red,
                Stat.PowerPoints => Color.yellow,
                Stat.MovementPoints => Color.green,
                Stat.Range => Color.cyan,
                _ => Color.white
            };
        }

        private void OnEnable()
        {
            battleChannel.statChangedEvent += OnStatChanged;
        }

        private void OnDisable()
        {
            battleChannel.statChangedEvent -= OnStatChanged;
        }
    }
}