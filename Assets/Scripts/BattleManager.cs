using System.Collections.Generic;
using System.Linq;
using Battlers;
using StateManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util;
using Utils;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private List<Battler> party;
    [SerializeField] private List<Battler> enemies;

    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap partyPlacementsTilemap;
    [SerializeField] private Tilemap enemyPlacementsTilemap;
    
    public List<Battler> Party => party;
    public List<Battler> Enemies => enemies;

    public Tilemap WalkableTilemap => walkableTilemap;
    public Tilemap PartyPlacementsTilemap => partyPlacementsTilemap;
    public Tilemap EnemyPlacementsTilemap => enemyPlacementsTilemap;

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
        StateManager.Instance.ChangeState<InitBattleState>();
    }
}
