using System.Collections.Generic;
using System.Linq;
using StateManagement;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Grid
{
    public class BattleGrid : NodeGrid
    {
        [SerializeField] private Tilemap partyPlacementsTilemap;
        [SerializeField] private Tilemap enemyPlacementsTilemap;

        [SerializeField] private Tilemap pathHighlightMap;
        [SerializeField] private TileBase pathHighlightTile;

        private BattleManager _battleManager; 
        
        public List<Vector2> PartyPlacements { get; private set; }
        public List<Vector2> EnemyPlacements { get; private set; }
        public List<Vector2> BattlerPositions => _battleManager.AllBattlers?.Select(b => b.Position).ToList();

        protected override void Awake()
        {
            base.Awake();
            _battleManager = GetComponent<BattleManager>();
            PartyPlacements = partyPlacementsTilemap.GetTilePositionsWorldSpace();
            EnemyPlacements = enemyPlacementsTilemap.GetTilePositionsWorldSpace();
        }
        
        public override List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) 
                        continue;
                    if (x != 0 && y != 0) 
                        continue;
                    int checkX = node.CellPositionX + x;
                    int checkY = node.CellPositionY + y;
                    Vector3Int vectorToAdd = new Vector3Int(checkX, checkY, 0);
                    if (NodeGridDictionary.ContainsKey(vectorToAdd)) 
                        neighbours.Add(NodeGridDictionary[vectorToAdd]);
                }
            }

            return neighbours;
        }

        public override bool ContainsBattler(Node node) => BattlerPositions.Contains(node.WorldPosition);

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
        
        public void HighlightPath(List<Node> path)
        {
            RemovePathHighlight();
            path?.ForEach(node => pathHighlightMap.SetTile(node.CellPosition, pathHighlightTile));
        }
        
        public void RemovePathHighlight() => pathHighlightMap.ClearAllTiles();
    }
}

