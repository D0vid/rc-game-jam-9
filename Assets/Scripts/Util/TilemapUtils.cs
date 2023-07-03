using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Util
{
    public static class TilemapUtils
    {
        private const float OffsetValue = 0.25f;
        
        public static Vector2 CellToWorldCentered(this Tilemap tilemap, Vector3Int cellPos)
        {
            return tilemap.CellToWorld(cellPos) + new Vector3(0, OffsetValue, 0);
        }
        
        public static List<Vector2> GetTilePositionsWorldSpace(this Tilemap tilemap)
        {
            var result = new List<Vector2>();
            tilemap.CompressBounds();
            BoundsInt cellBounds = tilemap.cellBounds;

            var allPositions = cellBounds.allPositionsWithin;

            foreach (var cellPos in allPositions)
            {
                if (tilemap.HasTile(cellPos))
                    result.Add(tilemap.CellToWorld(cellPos));
            }

            return result;
        }
    }
}