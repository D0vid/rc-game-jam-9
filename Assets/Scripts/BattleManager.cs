using System.Collections.Generic;
using Battlers;
using StateManagement;
using UnityEngine.Tilemaps;

public class BattleManager : StateManager
{
    // Everything is public so there is a cross reference between states & state machine, could be better with events
    public List<Battler> party;
    public List<Battler> enemies;
    
    public List<BattlerInstance> partyMembersInstances;
    public List<BattlerInstance> enemiesInstances;
    
    public Tilemap walkableTilemap;
    public Tilemap partyPlacementsTilemap;
    public Tilemap enemyPlacementsTilemap;

    private void Start()
    {
        ChangeState<InitBattleState>();
    }
}
