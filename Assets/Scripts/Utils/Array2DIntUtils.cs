﻿using UnityEngine;

namespace Utils
{
    public static class Array2DIntUtils
    {
        public static Vector2Int GetCoordinatesForValue<T>(this T[,] matrix, T value)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return new Vector2Int(x, y);
                }
            }
            return new Vector2Int(-1, -1);
        }
    }
}