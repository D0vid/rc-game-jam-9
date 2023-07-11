using System.Collections.Generic;
using StateManagement;
using UnityEngine;

namespace Grid
{
    public class PathfindingManager : MonoBehaviour
    {
        private NodeGrid _nodeGrid;

        private const int LinearWeight = 10;
        private const int SqrtWeight = 14;

        private void Awake()
        {
            _nodeGrid = GetComponent<NodeGrid>();
        }

        public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Node startNode = _nodeGrid.GetNodeForWorldPos(startPos);
            Node targetNode = _nodeGrid.GetNodeForWorldPos(targetPos);

            if (startNode == null || targetNode == null || startNode == targetNode)
                return new List<Node>();

            Heap<Node> openSet = new Heap<Node>(_nodeGrid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                    return RetracePath(startNode, targetNode);

                foreach (Node neighbour in _nodeGrid.GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || _nodeGrid.ContainsBattler(neighbour) || closedSet.Contains(neighbour)) 
                        continue;

                    int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    }
                }
            }
            return new List<Node>();
        }

        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.CellPositionX - nodeB.CellPositionX);
            int distanceY = Mathf.Abs(nodeA.CellPositionY - nodeB.CellPositionY);

            if (distanceX > distanceY)
            {
                return SqrtWeight * distanceY + LinearWeight * (distanceX - distanceY);
            }
            else
            {
                return SqrtWeight * distanceX + LinearWeight * (distanceY - distanceX);
            }
        }
    }
}
