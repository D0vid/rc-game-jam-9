using System.Collections.Generic;
using System.Linq;
using Battlers;
using Grid;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace StateManagement
{
    public class BattleManager : StateManager
    {
        [SerializeField] private List<Battler> party;
        [SerializeField] private List<Battler> enemies;
        
        public static new BattleManager Instance => (BattleManager)StateManager.Instance;

        public List<Battler> Party => party;
        public List<Battler> Enemies => enemies;

        public List<Vector2> PartyPlacements => _battleGrid.PartyPlacements;
        public List<Vector2> EnemyPlacements => _battleGrid.EnemyPlacements;

        public List<BattlerInstance> PartyBattlerInstances { get; set; }
        public List<BattlerInstance> EnemyBattlerInstances { get; set; }
        
        public List<BattlerInstance> AllBattlers => PartyBattlerInstances.Concat(EnemyBattlerInstances).ToList();
        
        public Queue<BattlerInstance> BattlersQueue { get; set; }

        public BattlerInstance CurrentBattler => BattlersQueue.Peek();
        
        private BattleGrid _battleGrid;

        protected override void Awake()
        {
            base.Awake();
            _battleGrid = GetComponent<BattleGrid>();
        }

        private void Start()
        {
            ChangeState<InitBattleState>();
        }

        public void InitBattlers(List<BattlerInstance> partyInstances, List<BattlerInstance> enemiesInstances)
        {
            PartyBattlerInstances = partyInstances;
            EnemyBattlerInstances = enemiesInstances;
        }

        public void StartPlayerOrEnemyTurn()
        {
            if (CurrentBattler.Team == Team.Party)
                ChangeState<PlayerTurnState>();
            else
                ChangeState<EnemyTurnState>();
        }

        public void EndTurn()
        {
            var currentTurnBattler = BattlersQueue.Dequeue();
            BattlersQueue.Enqueue(currentTurnBattler);
            StartPlayerOrEnemyTurn();
        }

        public void ShowPlacementPositions(bool show) => _battleGrid.ShowPlacementPositions(show);

        public Vector2 SnapPositionToGrid(Vector2 mousePos) => _battleGrid.SnapPositionToGrid(mousePos);
        
        public bool IsWalkable(Vector2 mousePos) => _battleGrid.IsWalkable(mousePos);

        public void HighlightPath(List<Node> path) => _battleGrid.HighlightPath(path);

        public void RemoveHighlights() => _battleGrid.RemovePathHighlight();
    }
}
