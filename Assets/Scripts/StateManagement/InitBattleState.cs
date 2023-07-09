using Battlers;
using Input;
using UnityEngine;

namespace StateManagement
{
    public class InitBattleState : State
    {
        private BattleManager _battleManager;
        private BattlersSpawner _battlersSpawner;
        private BattlersDragAndDropHandler _dragAndDropHandler;
        private TurnOrderResolver _turnOrderResolver;
        private InputChannel _inputChannel;

        private void Awake()
        {
            _battlersSpawner = new BattlersSpawner();
            _turnOrderResolver = new TurnOrderResolver();
            _inputChannel = Resources.Load("Input/InputChannel") as InputChannel;
        }

        public override void Enter()
        {
            base.Enter();
            _battleManager = BattleManager.Instance;
            _battleManager.ShowPlacementPositions(true);
            SpawnBattlers();
            _battleManager.BattlersQueue = _turnOrderResolver.ResolveTurnOrder(_battleManager.PartyBattlerInstances, _battleManager.EnemyBattlerInstances);
            _dragAndDropHandler = new BattlersDragAndDropHandler(_battleManager);
        }

        public override void Exit()
        {
            base.Exit();
            _battleManager.ShowPlacementPositions(false);
        }

        private void SpawnBattlers()
        {
            var party = _battlersSpawner.SpawnBattlers(_battleManager.Party, _battleManager.PartyPlacements, Team.Party);
            var enemies = _battlersSpawner.SpawnBattlers(_battleManager.Enemies, _battleManager.EnemyPlacements, Team.Enemies);
            _battleManager.InitBattlers(party, enemies);
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

        private void OnReady()
        {
            _battleManager.StartPlayerOrEnemyTurn();
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            _inputChannel.mouseBeginDragEvent += OnBeginMouseDrag;
            _inputChannel.mouseEndDragEvent += OnEndMouseDrag;
            _inputChannel.readySkipTurn += OnReady;
        }

        protected override void RemoveListeners()
        {
            base.RemoveListeners();
            _inputChannel.mouseBeginDragEvent -= OnBeginMouseDrag;
            _inputChannel.mouseEndDragEvent -= OnEndMouseDrag;
            _inputChannel.readySkipTurn -= OnReady;
        }
    }
}