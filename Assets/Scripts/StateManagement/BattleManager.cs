using System.Collections.Generic;
using System.Linq;
using Battlers;
using Grid;
using Input;
using UnityEngine;
using Utils;
using Vector2 = UnityEngine.Vector2;

namespace StateManagement
{
    public class BattleManager : StateManager
    {
        [SerializeField] private List<Battler> party;
        [SerializeField] private List<Battler> enemies;
        [SerializeField] private BattleChannel battleChannel;
        [SerializeField] private InputChannel inputChannel;
        
        public static new BattleManager Instance => (BattleManager)StateManager.Instance;

        public List<Battler> Party => party;
        public List<Battler> Enemies => enemies;

        public List<Vector2> PartyPlacements => _battleGrid.PartyPlacements;
        public List<Vector2> EnemyPlacements => _battleGrid.EnemyPlacements;

        public List<BattlerInstance> PartyBattlerInstances { get; private set; }
        public List<BattlerInstance> EnemyBattlerInstances { get; private set; }
        
        public IEnumerable<BattlerInstance> AllBattlers => PartyBattlerInstances.Concat(EnemyBattlerInstances).ToList();

        public BattlerInstance CurrentBattler => _battlersQueue.Peek();
        
        public int TurnNumber { get; set; }
        
        private BattleGrid _battleGrid;
        private Queue<BattlerInstance> _battlersQueue;

        protected override void Awake()
        {
            base.Awake();
            _battleGrid = GetComponent<BattleGrid>();
        }

        private void Start() => ChangeState<InitBattleState>();

        public void InitBattlers(List<BattlerInstance> partyInstances, List<BattlerInstance> enemiesInstances)
        {
            PartyBattlerInstances = partyInstances;
            EnemyBattlerInstances = enemiesInstances;
        }

        public void EndTurn()
        {
            if (TurnNumber > 0)
            {
                var currentTurnBattler = _battlersQueue.Dequeue();
                _battlersQueue.Enqueue(currentTurnBattler);
                currentTurnBattler.ResetStats();
            }
            battleChannel.RaiseCurrentBattlerChanged(_battlersQueue.Peek());
            TurnNumber++;
            DetermineNextTurn();
        }

        public void ShowPlacementPositions(bool show) => _battleGrid.ShowPlacementPositions(show);
        public Vector2 SnapPositionToGrid(Vector2 mousePos) => _battleGrid.SnapPositionToGrid(mousePos);
        public bool IsWalkable(Vector2 mousePos) => _battleGrid.IsWalkable(mousePos);
        public void HighlightPath(List<Node> path) => _battleGrid.HighlightPath(path);
        public void RemoveHighlights() => _battleGrid.RemoveAllHighlights();
        public List<Node> GetNodesInArea(Vector2 startingPos, int range, bool inLine) => _battleGrid.GetNodesInArea(startingPos, range, inLine);
        public Node GetNodeForWorldPos(Vector2 worldPos) => _battleGrid.GetNodeForWorldPos(worldPos);
        public List<Node> FilterTargetableNodes(Vector2 originPos, List<Node> nodes) => _battleGrid.FilterTargetableNodes(originPos, nodes);
        public void HighlightSkillRange(List<Node> nodesInRange, List<Node> targetableNodes) => _battleGrid.HighlightSkillRange(nodesInRange, targetableNodes);
        
        public void SimulateReadyEndTurn() => inputChannel.SimulateReadySkipTurn();

        private void OnTurnOrderResolved(Queue<BattlerInstance> battlersQueue) => _battlersQueue = battlersQueue;

        private void DetermineNextTurn()
        {
            if (CurrentBattler.Team == Team.Party)
                ChangeState<PlayerTurnState>();
            else
                ChangeState<EnemyTurnState>();
        }

        private void OnEnable() => battleChannel.turnOrderResolvedEvent += OnTurnOrderResolved;
        private void OnDisable() => battleChannel.turnOrderResolvedEvent -= OnTurnOrderResolved;
    }
}
