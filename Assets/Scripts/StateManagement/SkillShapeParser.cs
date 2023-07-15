using System.Collections.Generic;
using Battlers;
using Grid;
using UnityEngine;
using Utils;

namespace StateManagement
{
    public class SkillShapeParser
    {
        private const int CellValue = 2;
        private const int InfiniteLineValue = 3;
        private const int EmptyValue = 0;

        private readonly BattleManager _battleManager;

        public SkillShapeParser(BattleManager battleManager)
        {
            _battleManager = battleManager;
        }

        public List<Node> ParseSkill(Skill skill, Vector2 mousePos)
        {
            var nodeShape = new List<Node>();

            Array2DEditor.Array2DInt shape = skill.shape;

            var shapeArray = shape.GetCells();

            var shapeCenterCoords = shapeArray.GetCoordinatesForValue(1);
            Node originNode = _battleManager.GetNodeForWorldPos(mousePos);

            nodeShape.Add(originNode);

            for (int x = 0; x < shapeArray.GetLength(0); x++)
            {
                for (int y = 0; y < shapeArray.GetLength(1); y++)
                {
                    if (shapeArray[x, y] == CellValue && !skill.lineRestricted)
                        shapeCenterCoords = HandleCellValue(nodeShape, shapeCenterCoords, originNode, x, y);
                    else if (shapeArray[x, y] != EmptyValue && skill.lineRestricted)
                        shapeCenterCoords = HandleLineValue(shapeArray, nodeShape, shapeCenterCoords, originNode, x, y);
                }
            }

            return nodeShape;
        }

        private Vector2Int HandleCellValue(ICollection<Node> nodeShape, Vector2Int shapeCenterCoords, Node originNode, int x, int y)
        {
            int offsetX = x - shapeCenterCoords.x;
            int offsetY = y - shapeCenterCoords.y;
            Vector3Int cellCoords = originNode.CellPosition + new Vector3Int(offsetX, offsetY, 0);
            
            Node nodeToAdd = _battleManager.GetNodeForCellPos(cellCoords);
            if (nodeToAdd != null) 
                nodeShape.Add(nodeToAdd);

            return shapeCenterCoords;
        }

        private Vector2Int HandleLineValue(int[,] shapeArray, ICollection<Node> nodeShape, Vector2Int shapeCenterCoords,
            Node originNode, int x, int y)
        {
            int offsetX = x - shapeCenterCoords.x;
            int offsetY = y - shapeCenterCoords.y;

            Node battlerNode = _battleManager.GetNodeForWorldPos(_battleManager.CurrentBattler.Position);

            int diffX = originNode.CellPositionX - battlerNode.CellPositionX;
            int diffY = originNode.CellPositionY - battlerNode.CellPositionY;

            Vector3Int cellCoords;
            int max = shapeArray[x, y] == InfiniteLineValue ? 30 : 1;

            for (int i = 0; i < max; i++)
            {
                if (diffX > 0 && diffY == 0) // East
                    cellCoords = originNode.CellPosition + new Vector3Int(-offsetX + i, offsetY, 0);
                else if (diffX == 0 && diffY < 0) // South
                    cellCoords = originNode.CellPosition + new Vector3Int(offsetY, offsetX - i, 0);
                else if (diffX < 0 && diffY == 0) // West
                    cellCoords = originNode.CellPosition + new Vector3Int(offsetX - i, offsetY, 0);
                else // North
                    cellCoords = originNode.CellPosition + new Vector3Int(offsetY, -offsetX + i, 0);

                Node nodeToAdd = _battleManager.GetNodeForCellPos(cellCoords);
                if (nodeToAdd != null)
                    nodeShape.Add(nodeToAdd);
            }

            return shapeCenterCoords;
        }
    }
}