using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class NodeGrid : MonoBehaviour
    {
        [SerializeField] protected Tilemap walkableTilemap; 

        private int _gridSizeX, _gridSizeY;

        protected Dictionary<Vector3Int, Node> NodeGridDictionary;

        public int MaxSize => _gridSizeX * _gridSizeY;

        protected virtual void Awake()
        {
            CreateNodes();
        }

        public virtual List<Node> GetNeighbours(Node node)
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
                    if (NodeGridDictionary.ContainsKey(vectorToAdd)) 
                        neighbours.Add(NodeGridDictionary[vectorToAdd]);
                }
            }

            return neighbours;
        }

        public virtual bool ContainsBattler(Node neighbour) => false;

        public Node GetNodeForWorldPos(Vector3 worldPosition)
        {
            Vector3Int cellPos = walkableTilemap.WorldToCell(worldPosition);
            cellPos = new Vector3Int(cellPos.x, cellPos.y, 0);
            Node result = NodeGridDictionary.ContainsKey(cellPos) ? NodeGridDictionary[cellPos] : null;
            return result;
        }

        public bool IsWalkable(Vector2 worldPos)
        {
            Node node = GetNodeForWorldPos(worldPos);
            return node != null && node.Walkable;
        }

        private void CreateNodes()
        {
            walkableTilemap.CompressBounds();
            BoundsInt cellBounds = walkableTilemap.cellBounds;

            _gridSizeX = cellBounds.size.x;
            _gridSizeY = cellBounds.size.y;
            NodeGridDictionary = new Dictionary<Vector3Int, Node>();

            var allPositions = cellBounds.allPositionsWithin;

            foreach (var cellPos in allPositions)
            {
                var walkable = walkableTilemap.HasTile(cellPos);
                Vector3 worldPos = walkableTilemap.CellToWorld(cellPos);
                NodeGridDictionary[cellPos] = new Node(walkable, worldPos, cellPos);
            }
        }
    }
}