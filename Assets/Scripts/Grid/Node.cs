using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Grid
{
    public class Node : IHeapItem<Node>
    {
        public Node(bool walkable, Vector3 worldPos, Vector3Int cellPos)
        {
            Walkable = walkable;
            WorldPosition = worldPos;
            CellPosition = cellPos;
        }

        public bool Walkable { get; private set; }

        public Vector3 WorldPosition { get; private set; }

        public float WorldPositionX { get => WorldPosition.x; }
        public float WorldPositionY { get => WorldPosition.y; }

        public Vector3Int CellPosition { get; private set; }

        public int CellPositionX { get => CellPosition.x; }
        public int CellPositionY { get => CellPosition.y; }

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get => GCost + HCost; }

        public Node Parent { get; set; }

        public int HeapIndex { get; set; }

        private Vector2 SouthEdge => new Vector2(WorldPositionX, WorldPositionY);
        private Vector2 EastEdge => new Vector2(WorldPositionX + 0.5f, WorldPositionY + 0.25f);
        private Vector2 NorthEdge => new Vector2(WorldPositionX, WorldPositionY + 0.5f);
        private Vector2 WestEdge => new Vector2(WorldPositionX - 0.5f, WorldPositionY + 0.25f);

        public List<Vector2> Edges => new List<Vector2>() { SouthEdge, EastEdge, NorthEdge, WestEdge };

        public bool IsEdge(Vector2 point) => SouthEdge == point || EastEdge == point || NorthEdge == point || WestEdge == point;

        public List<Line> Lines
        {
            get
            {
                return new List<Line> 
                {
                    new Line(SouthEdge, WestEdge),
                    new Line(WestEdge, NorthEdge),
                    new Line(NorthEdge, EastEdge),
                    new Line(EastEdge, SouthEdge)
                };
            }
        }

        public int CompareTo(Node other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }
            return -compare;
        }

        public override string ToString() => $"({WorldPositionX}, {WorldPositionY})";
    }
}
