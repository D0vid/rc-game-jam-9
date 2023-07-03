using System.Collections.Generic;
using Battlers;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util;

namespace StateManagement
{
    public class InitBattleState : State
    {
        private Tilemap _partyPlacementsTilemap;
        private Tilemap _enemyPlacementsTilemap;

        private List<Battler> _party;
        private List<Battler> _enemies;

        private BattlersSpawner _battlersSpawner;

        private void Awake()
        {
            var battleManager = BattleManager.Instance;
            
            _partyPlacementsTilemap = battleManager.partyPlacementsTilemap;
            _enemyPlacementsTilemap = battleManager.enemyPlacementsTilemap;
            
            _party = battleManager.party;
            _enemies = battleManager.enemies;
            
            _battlersSpawner = new BattlersSpawner();
        }

        public override void Enter()
        {
            base.Enter();
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
            Debug.Log("> Now in InitBattleState");
            ShowPlacements(true);
            SpawnBattlers();
        }

        private void ShowPlacements(bool showPlacements)
        {
            _partyPlacementsTilemap.gameObject.SetActive(showPlacements);
            _enemyPlacementsTilemap.gameObject.SetActive(showPlacements);
        }

        private void SpawnBattlers()
        {
            BattleManager.Instance.partyMembersInstances = _battlersSpawner.SpawnBattlers(_party, _partyPlacementsTilemap.GetTilePositionsWorldSpace());
            BattleManager.Instance.enemiesInstances = _battlersSpawner.SpawnBattlers(_enemies, _enemyPlacementsTilemap.GetTilePositionsWorldSpace());
        }
    }
}