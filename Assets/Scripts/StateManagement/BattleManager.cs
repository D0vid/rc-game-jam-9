using System.Collections.Generic;
using System.Linq;
using Battlers;
using UnityEngine;
using Utils;

namespace StateManagement
{
    public class BattleManager : StateManager
    {
        [SerializeField] private List<Battler> party;
        [SerializeField] private List<Battler> enemies;

        public List<Battler> Party => party;
        public List<Battler> Enemies => enemies;

        public List<BattlerInstance> PartyBattlerInstances { get; set; }
        public List<BattlerInstance> EnemyBattlerInstances { get; set; }
        public List<BattlerInstance> AllBattlers => PartyBattlerInstances.Concat(EnemyBattlerInstances).ToList();
        public PriorityQueue<BattlerInstance, int> BattlersQueue { get; set; }

        private TurnOrderResolver _turnOrderResolver;

        protected override void Awake()
        {
            base.Awake();
            _turnOrderResolver = new TurnOrderResolver();
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
    }
}
