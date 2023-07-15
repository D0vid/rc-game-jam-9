using System;
using System.Collections;
using System.Collections.Generic;
using Battlers;
using Grid;
using Input;
using UnityEngine;
using Utils;

namespace StateManagement
{
    public class PlayerTurnState : State
    {
        private InputChannel _inputChannel;
        private BattleChannel _battleChannel;

        private BattleManager _battleManager;
        private PathfindingManager _pathFindingManager;
        private SkillCastHandler _skillCastHandler;

        private Vector2 _lastSentPosition;
        private BattlerInstance _currentBattler;
        private List<Node> _currentPath;

        private Coroutine _movementCoroutine;

        private void Awake()
        {
            _inputChannel = Resources.Load<InputChannel>("Channels/InputChannel");
            _battleChannel = Resources.Load<BattleChannel>("Channels/BattleChannel");
            _pathFindingManager = GetComponent<PathfindingManager>();
        }

        public override void Enter()
        {
            _battleManager = BattleManager.Instance;
            _skillCastHandler = new SkillCastHandler(_battleManager);
            _currentBattler = _battleManager.CurrentBattler;
            base.Enter();
            Debug.Log($"> Now in PlayerTurnState - Battler : {_battleManager.CurrentBattler.name}");
        }

        public override void Exit()
        {
            base.Exit();
            ResetPath();
        }

        private void OnMouseHover(Vector2 mousePos)
        {
            Vector2 mousePosSnapped = _battleManager.SnapPositionToGrid(mousePos);
            if (_currentBattler.State == BattlerState.Idle)
            {
                if (_lastSentPosition == mousePosSnapped)
                    return;
                var currentBattlerPos = _currentBattler.Position;
                _currentPath = _pathFindingManager.FindPath(currentBattlerPos, mousePosSnapped);
                _lastSentPosition = mousePosSnapped;
                if (_currentPath.Count <= _currentBattler.CurrentMP)
                    _battleManager.HighlightPath(_currentPath);
                else
                    ResetPath();
            }
        }

        private void OnMouseClick(Vector2 mousePos)
        {
            Vector2 mousePosSnapped = _battleManager.SnapPositionToGrid(mousePos);
            if (_currentBattler.State == BattlerState.Idle)
            {
                if (!(_currentPath?.Count > 0) || mousePosSnapped != _lastSentPosition)
                    return;
                _movementCoroutine = StartCoroutine(_currentBattler.FollowPath(_currentPath, OnEndOfPathReached));
            }
        }

        private void OnEndOfPathReached() => ResetPath();

        private void ResetPath()
        {
            _battleManager.RemoveAllHighlights();
            _currentPath?.Clear();
        }

        private void OnEndTurn()
        {
            StartCoroutine(WaitForCoroutinesAndEndTurn(() => _battleManager.EndTurn()));
        }

        private IEnumerator WaitForCoroutinesAndEndTurn(Action onCoroutinesEnded)
        {
            if (_movementCoroutine != null)
                yield return _movementCoroutine;
            onCoroutinesEnded();
        }

        private void OnSkillSelected(Skill skill)
        {
            if (!_currentBattler.Skills.Contains(skill))
                return;
            if (_currentBattler.State != BattlerState.Moving)
                _skillCastHandler.HandleSkillSelected(skill);
        }

        private void OnHotkeyPressed(int key)
        {
            var skills = _currentBattler.Skills;
            if (skills.Count < key)
                return;
            OnSkillSelected(skills[key - 1]);
        }

        private void OnActionCancelled()
        {
            _currentBattler.State = BattlerState.Idle;
            _battleManager.RemoveAllHighlights();
        }

        protected override void AddListeners()
        {
            _inputChannel.mousePositionEvent += OnMouseHover;
            _inputChannel.mousePositionEvent += _skillCastHandler.OnMouseHover;
            _inputChannel.mouseClickEvent += OnMouseClick;
            _inputChannel.mouseClickEvent += _skillCastHandler.OnMouseClick;
            _inputChannel.readySkipTurn += OnEndTurn;
            _battleChannel.skillSelectedEvent += OnSkillSelected;
            _inputChannel.hotkeyPressed += OnHotkeyPressed;
            _inputChannel.actionCanceled += OnActionCancelled;
        }

        protected override void RemoveListeners()
        {
            _inputChannel.mousePositionEvent -= OnMouseHover;
            _inputChannel.mousePositionEvent -= _skillCastHandler.OnMouseHover;
            _inputChannel.mouseClickEvent -= OnMouseClick;
            _inputChannel.mouseClickEvent -= _skillCastHandler.OnMouseClick;
            _inputChannel.readySkipTurn -= OnEndTurn;
            _battleChannel.skillSelectedEvent -= OnSkillSelected;
            _inputChannel.hotkeyPressed -= OnHotkeyPressed;
            _inputChannel.actionCanceled -= OnActionCancelled;
        }
    }
}