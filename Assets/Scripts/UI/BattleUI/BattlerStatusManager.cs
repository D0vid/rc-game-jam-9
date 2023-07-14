using System;
using System.Collections.Generic;
using Battlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.BattleUI
{
    public class BattlerStatusManager : MonoBehaviour
    {
        [SerializeField] private BattleChannel battleChannel;

        [SerializeField] private Image portrait;
        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI battlerName;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image type1Sprite;
        [SerializeField] private Image type2Sprite;
        [SerializeField] private TextMeshProUGUI ppText;
        [SerializeField] private TextMeshProUGUI raText;
        [SerializeField] private TextMeshProUGUI mpText;
        [SerializeField] private TextMeshProUGUI shText;

        private BattlerInstance _currentBattler;
        private BattlerInstance _displayedBattler;
        private BattlerInstance _hoveredBattler;
        private List<BattlerInstance> _battlers;

        private void Update()
        {
            if (_battlers == null || _currentBattler == null)
                return;

            _displayedBattler = _hoveredBattler ? _hoveredBattler : _currentBattler;

            UpdateBattlerStatus(_displayedBattler);
        }

        private void UpdateBattlerStatus(BattlerInstance displayedBattler)
        {
            portrait.sprite = displayedBattler.battler.Portrait;
            healthBar.fillAmount = displayedBattler.PercentHP;
            battlerName.text = displayedBattler.battler.Name;
            healthText.text = $"{displayedBattler.CurrentHP} / {displayedBattler.MaxHP}";
            type1Sprite.sprite = displayedBattler.battler.Typing[0].icon;
            if (displayedBattler.battler.Typing.Length > 1)
                type2Sprite.sprite = displayedBattler.battler.Typing[1].icon;
            ppText.text = displayedBattler.CurrentPP.ToString();
            raText.text = "0";
            mpText.text = displayedBattler.CurrentMP.ToString();
            shText.text = "0";
        }

        private void OnCurrentBattlerChanged(BattlerInstance newBattler)
        {
            _currentBattler = newBattler;
            transform.Find("BattlerStatus").gameObject.SetActive(true);
            transform.Find("Button/ReadyEndText").GetComponent<TextMeshProUGUI>().text = "End Turn";
        }

        private void OnTurnOrderResolved(Queue<BattlerInstance> battlers)
        {
            _battlers = new List<BattlerInstance>(battlers);
        }

        private void OnStartedHoveringBattler(BattlerInstance battler) => _hoveredBattler = battler;

        private void OnStoppedHoveringBattler(BattlerInstance battler) => _hoveredBattler = null;

        private void OnEnable()
        {
            battleChannel.currentBattlerChangedEvent += OnCurrentBattlerChanged;
            battleChannel.turnOrderResolvedEvent += OnTurnOrderResolved;
            battleChannel.startedHoveringBattlerEvent += OnStartedHoveringBattler;
            battleChannel.stoppedHoveringBattlerEvent += OnStoppedHoveringBattler;
        }

        private void OnDisable()
        {
            battleChannel.currentBattlerChangedEvent -= OnCurrentBattlerChanged;
            battleChannel.turnOrderResolvedEvent -= OnTurnOrderResolved;
            battleChannel.startedHoveringBattlerEvent -= OnStartedHoveringBattler;
            battleChannel.stoppedHoveringBattlerEvent -= OnStoppedHoveringBattler;
        }
    }
}
