using System;
using System.Collections;
using System.Collections.Generic;
using Battlers;
using Grid;
using Input;
using UnityEngine;

namespace StateManagement
{
    public class PlayerTurnState : State
    {
        private BattleManager _battleManager;
        private PathfindingManager _pathFindingManager;

        private Vector2 _lastSentPosition;
        private InputChannel _inputChannel;
        private BattlerInstance _currentBattler;
        private List<Node> _currentPath;

        private Coroutine _movementCoroutine;

        private void Awake()
        {
            _inputChannel = Resources.Load<InputChannel>("Input/InputChannel");
            _pathFindingManager = GetComponent<PathfindingManager>();
        }

        public override void Enter()
        {
            base.Enter();
            _battleManager = BattleManager.Instance;
            _currentBattler = _battleManager.CurrentBattler;
            _currentBattler.InitStatsForNewTurn();
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
            _battleManager.RemoveHighlights();
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

        protected override void AddListeners()
        {
            _inputChannel.mousePositionEvent += OnMouseHover;
            _inputChannel.mouseClickEvent += OnMouseClick;
            _inputChannel.readySkipTurn += OnEndTurn;
        }

        protected override void RemoveListeners()
        {
            _inputChannel.mousePositionEvent -= OnMouseHover;
            _inputChannel.mouseClickEvent -= OnMouseClick;
            _inputChannel.readySkipTurn -= OnEndTurn;
        }
    }
}