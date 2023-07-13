using System.Collections.Generic;
using Battlers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.BattleUI
{
    public class TimeLineManager : MonoBehaviour
    {
        [SerializeField] private GameObject timelineAllyPrefab;
        [SerializeField] private GameObject timelineEnemyPrefab;
        [SerializeField] private BattleChannel battleChannel;

        private Dictionary<int, GameObject> _arrows;

        private void Awake() => _arrows = new Dictionary<int, GameObject>();

        private void OnTurnOrderResolved(Queue<BattlerInstance> battlers)
        {
            var queue = new Queue<BattlerInstance>(battlers); // Need a copy else we literally pop the real battlers
            while (queue.Count > 0)
            {
                BattlerInstance battlerInstance = queue.Dequeue();
                var prefabToInstantiate = battlerInstance.Team == Team.Party ? timelineAllyPrefab : timelineEnemyPrefab;
                GameObject timelineBattler = Instantiate(prefabToInstantiate, transform);
                timelineBattler.name = battlerInstance.name;
                var timelineSprite = timelineBattler.transform.Find("BattlerCard/Background/Sprite").GetComponent<Image>();
                timelineSprite.sprite = battlerInstance.battler.TimelineSprite;
                _arrows.Add(battlerInstance.GetInstanceID(), timelineBattler.transform.Find("Arrow").gameObject);
            }
        }

        private void OnCurrentBattlerChanged(BattlerInstance newBattler)
        {
            var newBattlerId = newBattler.GetInstanceID();
            foreach (var arrow in _arrows)
            {
                arrow.Value.SetActive(arrow.Key == newBattlerId);
            }
        }

        private void OnEnable()
        {
            battleChannel.turnOrderResolvedEvent += OnTurnOrderResolved;
            battleChannel.currentBattlerChangedEvent += OnCurrentBattlerChanged;
        }

        private void OnDisable()
        {
            battleChannel.turnOrderResolvedEvent -= OnTurnOrderResolved;
            battleChannel.currentBattlerChangedEvent -= OnCurrentBattlerChanged;
        }
    }
}
