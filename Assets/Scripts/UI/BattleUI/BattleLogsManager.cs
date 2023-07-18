using System.Collections;
using System.Collections.Generic;
using Battlers;
using Grid;
using TMPro;
using UnityEngine;
using Utils.Channels;

namespace UI.BattleUI
{
    public class BattleLogsManager : MonoBehaviour
    {
        private const int MaxMessages = 8;
        
        [SerializeField] private BattleChannel battleChannel;

        private Queue<string> _messageQueue;
        private TextMeshProUGUI _textArea;
        
        private void Awake()
        {
            _messageQueue = new Queue<string>();
            _textArea = GetComponent<TextMeshProUGUI>();
        }

        private void LogMessage(string message)
        {
            _messageQueue.Enqueue(message);

            if (_messageQueue.Count > MaxMessages)
            {
                _messageQueue.Dequeue();
            }

            UpdateTextArea();
        }

        private void UpdateTextArea()
        {
            _textArea.text = "";
            foreach (var message in _messageQueue)
            {
                _textArea.text += "> " + message + "\n";
            }
        }

        private void OnSkillCast(BattlerInstance source, Skill skill, List<Node> aoe, IEnumerable<BattlerInstance> aliveBattlers)
        {
            var team = source.Team == Team.Enemies ? "Enemy " : "";
            var battlerName = source.name;
            var skillName = skill.name;
            var message = $"{team}{battlerName} used {skillName}."; // TODO https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html
            LogMessage(message);
        }

        private void OnStatChanged(BattlerInstance battler, Stat stat, int amount)
        {
            if (stat == Stat.Health)
            {
                var sign = amount > 0 ? "+" : "";
                var message = $"{battler.name}: {sign}{amount} HP.";
                LogMessage(message);
            }
        }

        private void OnBattlerFainted(BattlerInstance battler)
        {
            var team = battler.Team == Team.Enemies ? "Enemy " : "";
            var message = $"{team}{battler.name} Fainted.";
            LogMessage(message);
        }


        private void OnEnable()
        {
            battleChannel.skillCastEvent += OnSkillCast;
            battleChannel.statChangedEvent += OnStatChanged;
            battleChannel.battlerFaintedEvent += OnBattlerFainted;
        }

        private void OnDisable()
        {
            battleChannel.skillCastEvent -= OnSkillCast;
            battleChannel.statChangedEvent -= OnStatChanged;
            battleChannel.battlerFaintedEvent -= OnBattlerFainted;
        }
    }
}