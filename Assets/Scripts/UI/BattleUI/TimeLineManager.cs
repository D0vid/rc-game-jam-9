using System.Collections.Generic;
using System.Linq;
using Battlers;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Channels;

namespace UI.BattleUI
{
    public class TimeLineManager : MonoBehaviour
    {
        [SerializeField] private GameObject timelineAllyPrefab;
        [SerializeField] private GameObject timelineEnemyPrefab;
        [SerializeField] private BattleChannel battleChannel;

        private Dictionary<int, GameObject> _timelineBattlers;

        private void Awake() => _timelineBattlers = new Dictionary<int, GameObject>();

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
                _timelineBattlers.Add(battlerInstance.GetInstanceID(), timelineBattler.gameObject);
            }
        }

        private void OnCurrentBattlerChanged(BattlerInstance newBattler)
        {
            var newBattlerId = newBattler.GetInstanceID();
            foreach (var timelineBattler in _timelineBattlers)
            {
                timelineBattler.Value.transform.Find("Arrow").gameObject.SetActive(timelineBattler.Key == newBattlerId);
            }
        }

        private void OnBattlerFaintedEvent(BattlerInstance faintedBattler)
        {
            var timelineBattler = GetTimelineBattlerObject(faintedBattler);
            timelineBattler.transform.Find($"BattlerCard/FaintedOverlay").GetComponent<Image>().enabled = true;
        }

        private void OnStatChanged(BattlerInstance battler, Stat stat, int amount)
        {
            if (stat == Stat.Health)
            {
                var timelineBattler = GetTimelineBattlerObject(battler);
                var image = timelineBattler.transform.Find($"BattlerCard/HealthBar").GetComponent<Image>();
                image.fillAmount = battler.PercentHp;
                image.color = Color.Lerp(new Color(0.647f, 0.188f, 0.188f), new Color(0.459f, 0.655f, 0.263f), image.fillAmount);
            }
        }

        private GameObject GetTimelineBattlerObject(BattlerInstance battler)
        {
            return _timelineBattlers
                .Where(tb => tb.Key == battler.GetInstanceID())
                .Select(kvp => kvp.Value)
                .First();
        }

        private void OnEnable()
        {
            battleChannel.turnOrderResolvedEvent += OnTurnOrderResolved;
            battleChannel.currentBattlerChangedEvent += OnCurrentBattlerChanged;
            battleChannel.battlerFaintedEvent += OnBattlerFaintedEvent;
            battleChannel.statChangedEvent += OnStatChanged;
        }

        private void OnDisable()
        {
            battleChannel.turnOrderResolvedEvent -= OnTurnOrderResolved;
            battleChannel.currentBattlerChangedEvent -= OnCurrentBattlerChanged;
            battleChannel.battlerFaintedEvent -= OnBattlerFaintedEvent;
            battleChannel.statChangedEvent -= OnStatChanged;
        }
    }
}
