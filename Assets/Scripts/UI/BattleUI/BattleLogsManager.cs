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
        [SerializeField] private BattleChannel battleChannel;

        private Stack<string> _messages;
        private bool _newMessage;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _messages = new Stack<string>();
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (_newMessage)
            {
                _text.text = "";
                foreach (var msg in _messages)
                {
                    _text.text = msg + _text.text;
                }
                _newMessage = false;
            }
        }

        private void AddMessage(string message)
        {
            if (_messages.Count > 8)
                _messages.Pop();

            _messages.Push($"> {message}.\n");
            _newMessage = true;
        }

        private void OnSkillCast(BattlerInstance source, Skill skill, List<Node> aoe, IEnumerable<BattlerInstance> aliveBattlers)
        {
            var team = source.Team == Team.Enemies ? "Enemy " : "";
            var battlerName = source.name;
            var skillName = skill.name;
            var message =
                $"{team}{battlerName} used {skillName}"; // TODO https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/StyledText.html
            AddMessage(message);
        }

        private void OnStatChanged(BattlerInstance battler, Stat stat, int amount)
        {
            if (stat == Stat.Health)
            {
                var sign = amount < 0 ? "+" : "-";
                var message = $"{battler.name}: {sign}{Mathf.Abs(amount)} HP";
                AddMessage(message);
            }
        }

        private void OnBattlerFainted(BattlerInstance battler)
        {
            var team = battler.Team == Team.Enemies ? "Enemy " : "";
            var message = $"{team}{battler.name} Fainted";
            AddMessage(message);
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