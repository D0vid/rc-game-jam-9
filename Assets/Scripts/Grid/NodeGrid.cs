using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Grid
{
    public class NodeGrid : Singleton<NodeGrid>
    {
        [SerializeField] protected Tilemap walkableTilemap; 

        private Dictionary<Vector3Int, Node> _nodeGridDictionary;

        private int _gridSizeX, _gridSizeY;

        protected override void Awake()
        {
            base.Awake();
            CreateNodes();
        }

        public int MaxSize => _gridSizeX * _gridSizeY;

        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    var checkX = node.CellPositionX + x;
                    var checkY = node.CellPositionY + y;
                    Vector3Int vectorToAdd = new Vector3Int(checkX, checkY, 0);
                    if (_nodeGridDictionary.ContainsKey(vectorToAdd)) 
                        neighbours.Add(_nodeGridDictionary[vectorToAdd]);
                }
            }

            return neighbours;
        }

        public Node GetNodeForWorldPos(Vector3 worldPosition)
        {
            Vector3Int cellPos = walkableTilemap.WorldToCell(worldPosition);
            cellPos = new Vector3Int(cellPos.x, cellPos.y, 0);
            Node result = _nodeGridDictionary.ContainsKey(cellPos) ? _nodeGridDictionary[cellPos] : null;
            return result;
        }

        public bool IsWalkable(Vector2 worldPos)
        {
            Node node = GetNodeForWorldPos(worldPos);
            return node != null && node.Walkable;
        }

        protected void CreateNodes()
        {
            walkableTilemap.CompressBounds();
            BoundsInt cellBounds = walkableTilemap.cellBounds;

            _gridSizeX = cellBounds.size.x;
            _gridSizeY = cellBounds.size.y;
            _nodeGridDictionary = new Dictionary<Vector3Int, Node>();

            var allPositions = cellBounds.allPositionsWithin;

            foreach (var cellPos in allPositions)
            {
                var walkable = walkableTilemap.HasTile(cellPos);
                Vector3 worldPos = walkableTilemap.CellToWorld(cellPos);
                _nodeGridDictionary[cellPos] = new Node(walkable, worldPos, cellPos);
            }
        }
    }
}