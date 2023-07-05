using System;
using System.Collections.Generic;
using Battlers;
using Input;
using UnityEngine;
using Utils;

namespace StateManagement
{
    public class InitBattleState : State
    {
        private BattleManager _battleManager;
        private BattlersSpawner _battlersSpawner;
        private BattlersDragAndDropHandler _dragAndDropHandler;

        private List<Vector2> _partyPositions;
        private List<Vector2> _enemyPositions;

        private bool _battlersInitialized;

        private List<BattlerInstance> _party;
        private List<BattlerInstance> _enemies;

        private InputChannel _inputChannel;

        private void Awake()
        {
            _battleManager = BattleManager.Instance;
            _battlersSpawner = new BattlersSpawner();
            _partyPositions = _battleManager.PartyPlacementsTilemap.GetTilePositionsWorldSpace();
            _enemyPositions = _battleManager.EnemyPlacementsTilemap.GetTilePositionsWorldSpace();
            _inputChannel = Resources.Load("Input/InputChannel") as InputChannel;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("> Now in InitBattleState");
            InitBattle();
        }

        public override void Exit()
        {
            base.Exit();
            ShowPlacements(false);
            StateManager.Instance.ChangeState<PlayerTurnState>();
        }

        private void InitBattle()
        {
            ShowPlacements(true);
            SpawnBattlers();
            _dragAndDropHandler = new BattlersDragAndDropHandler(_party, _partyPositions, _battleManager.WalkableTilemap);
        }

        private void ShowPlacements(bool showPlacements)
        {
            _battleManager.PartyPlacementsTilemap.gameObject.SetActive(showPlacements);
            _battleManager.EnemyPlacementsTilemap.gameObject.SetActive(showPlacements);
        }

        private void SpawnBattlers()
        {
            _party = _battlersSpawner.SpawnBattlers(_battleManager.Party, _partyPositions);
            _enemies = _battlersSpawner.SpawnBattlers(_battleManager.Enemies, _enemyPositions);
            _battleManager.PartyBattlerInstances = _party;
            _battleManager.EnemyBattlerInstances = _enemies;
        }
        
        private void OnBeginMouseDrag(Vector2 mousePos)
        {
            _dragAndDropHandler.OnBeginMouseDrag(mousePos);
            _inputChannel.mousePositionEvent += _dragAndDropHandler.OnMouseDrag;
        }

        private void OnEndMouseDrag(Vector2 mousePos)
        {
            _dragAndDropHandler.OnEndMouseDrag(mousePos);
            _inputChannel.mousePositionEvent -= _dragAndDropHandler.OnMouseDrag;
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            _inputChannel.mouseBeginDragEvent += OnBeginMouseDrag;
            _inputChannel.mouseEndDragEvent += OnEndMouseDrag;
            _inputChannel.readySkipTurn += Exit;
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            _inputChannel.mouseBeginDragEvent -= OnBeginMouseDrag;
            _inputChannel.mouseEndDragEvent -= OnEndMouseDrag;
            _inputChannel.readySkipTurn -= Exit;
        }
    }
}