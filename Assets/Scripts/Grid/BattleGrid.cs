using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Utils;

namespace Grid
{
    public class BattleGrid : NodeGrid
    {
        [SerializeField] private Tilemap partyPlacementsTilemap;
        [SerializeField] private Tilemap enemyPlacementsTilemap;
        
        public List<Vector2> PartyPlacements { get; private set; }
        public List<Vector2> EnemyPlacements { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            CreateNodes();
            PartyPlacements = partyPlacementsTilemap.GetTilePositionsWorldSpace();
            EnemyPlacements = enemyPlacementsTilemap.GetTilePositionsWorldSpace();
        }

        public void ShowPlacementPositions(bool show)
        {
            partyPlacementsTilemap.GetComponent<TilemapRenderer>().enabled = show;
            enemyPlacementsTilemap.GetComponent<TilemapRenderer>().enabled = show;
        }

        public Vector2 SnapPositionToGrid(Vector2 position)
        {
            Node node = GetNodeForWorldPos(position);
            return node?.WorldPosition ?? position;
        }
    }
}

