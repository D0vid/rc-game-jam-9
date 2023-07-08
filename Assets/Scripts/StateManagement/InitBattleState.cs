using System.Linq;
using Grid;
using Input;
using UnityEngine;

namespace StateManagement
{
    public class InitBattleState : State
    {
        private BattleManager _battleManager;
        private BattleGrid _battleGrid;
        private BattlersSpawner _battlersSpawner;
        private BattlersDragAndDropHandler _dragAndDropHandler;

        private InputChannel _inputChannel;

        private void Awake()
        {
            _battlersSpawner = new BattlersSpawner();
            _inputChannel = Resources.Load("Input/InputChannel") as InputChannel;
        }

        public override void Enter()
        {
            base.Enter();
            _battleManager = BattleManager.Instance as BattleManager;
            _battleGrid = BattleGrid.Instance as BattleGrid;
            _battleGrid!.ShowPlacementPositions(true);
            SpawnBattlers();
            _dragAndDropHandler = new BattlersDragAndDropHandler(_battleGrid);
            _dragAndDropHandler.Party = _battleManager!.PartyBattlerInstances;
        }

        public override void Exit()
        {
            base.Exit();
            _battleGrid.ShowPlacementPositions(false);
            StateManager.Instance.ChangeState<PlayerTurnState>();
        }

        private void SpawnBattlers()
        {
            var party = _battlersSpawner.SpawnBattlers(_battleManager.Party, _battleGrid.PartyPlacements);
            var enemies = _battlersSpawner.SpawnBattlers(_battleManager.Enemies, _battleGrid.EnemyPlacements);
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