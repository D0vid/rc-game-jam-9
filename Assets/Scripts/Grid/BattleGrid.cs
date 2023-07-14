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

        [SerializeField] private Tilemap rangeHighlightMap;
        [SerializeField] private TileBase rangeHighlightTile;
        [SerializeField] private TileBase untargetableHighlightTile;

        [SerializeField] private Tilemap obstaclesMap;

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
            RemoveAllHighlights();
            path?.ForEach(node => pathHighlightMap.SetTile(node.CellPosition, pathHighlightTile));
        }

        public void HighlightSkillRange(List<Node> nodesInRange, List<Node> targetableNodes)
        {
            RemoveAllHighlights();
            nodesInRange.ForEach(node =>
            {
                if (walkableTilemap.HasTile(node.CellPosition))
                    rangeHighlightMap.SetTile(node.CellPosition, rangeHighlightTile);
                if (!targetableNodes.Contains(node))
                    rangeHighlightMap.SetTile(node.CellPosition, untargetableHighlightTile);
            });
        }

        public void RemoveAllHighlights()
        {
            pathHighlightMap.ClearAllTiles();
            rangeHighlightMap.ClearAllTiles();
        }

        public List<Node> GetNodesInArea(Vector2 startingPos, int range, bool inLine)
        {
            return inLine ? GetNodesInLines(startingPos, range) : GetNodesInCircle(startingPos, range);
        }

        private List<Node> GetNodesInLines(Vector2 startingPos, int range)
        {
            var result = new List<Node>();
            for (int i = 0; i < range; i++)
            {
                result.Add(GetNodeForWorldPos(new Vector2(startingPos.x + (i * 0.5f), startingPos.y + (i * 0.25f))));
                result.Add(GetNodeForWorldPos(new Vector2(startingPos.x + (i * 0.5f), startingPos.y + (i * -0.25f))));
                result.Add(GetNodeForWorldPos(new Vector2(startingPos.x + (i * -0.5f), startingPos.y + (i * -0.25f))));
                result.Add(GetNodeForWorldPos(new Vector2(startingPos.x + (i * -0.5f), startingPos.y + (i * 0.25f))));
            }
            result.RemoveAll(node => node == null);
            return result;
        }

        private List<Node> GetNodesInCircle(Vector2 startingPos, int range)
        {
            Node startingNode = GetNodeForWorldPos(startingPos);
            var result = new HashSet<Node>() { startingNode };
            for (int i = 0; i < range; i++)
            {
                var tempResult = new List<Node>(result);
                foreach (Node node in tempResult)
                {
                    result.UnionWith(GetNeighbours(node));
                }
            }
            result.RemoveWhere(node => !node.Walkable || !walkableTilemap.HasTile(node.CellPosition) || node == startingNode);
            return new List<Node>(result);
        }

        public List<Node> FilterTargetableNodes(Vector2 originPos, List<Node> nodes)
        {
            var result = new HashSet<Node>(nodes);

            Node startingNode = GetNodeForWorldPos(originPos);

            var obstacles = AllNodes
                .Where(node => node != startingNode && (obstaclesMap.HasTile(node.CellPosition) || BattlerPositions.Contains(node.WorldPosition)))
                .ToList();

            foreach (var node in nodes)
            {
                Line sightLine = new Line(originPos.AddOffSet(), node.WorldPosition.AddOffSet());
                foreach (var obstacleNode in obstacles)
                {
                    if (!result.Contains(node))
                        break;
                    var obstacleLines = GetObstacleLines(obstacleNode, node, originPos);
                    var edgesHit = 0;
                    foreach (var line in obstacleLines)
                    {
                        if (sightLine.Intersects(line, out var intersection))
                        {
                            if (!obstacleNode.IsEdge(intersection))
                                result.Remove(node);
                            else
                                edgesHit++;
                        }
                    }

                    if (edgesHit == obstacleLines.Count)
                        result.Remove(node);
                }
                // Debug.DrawLine(sightLine.Start, sightLine.End, result.Contains(node) ? Color.green : Color.red, 5f);
            }

            return new List<Node>(result);
        }

        private List<Line> GetObstacleLines(Node obstacleNode, Node targetNode, Vector2 startingPos)
        {
            var result = obstacleNode.Lines;
            bool obstacleIsBattler = BattlerPositions.Contains(obstacleNode.WorldPosition);
            bool targetIsBattler = BattlerPositions.Contains(targetNode.WorldPosition);
            if (obstacleIsBattler && targetIsBattler)
            {
                var edges = obstacleNode.Edges;
                edges.Sort((e1, e2) => Vector2.Distance(e1, startingPos).CompareTo(Vector2.Distance(e2, startingPos)));
                var closestLine = result.Single(line =>
                    (line.Start == edges[0] && line.End == edges[1]) ||
                    (line.Start == edges[1] && line.End == edges[0]));
                result.Remove(closestLine);
            }

            return result;
        }
    }
}