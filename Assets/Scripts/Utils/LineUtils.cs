using UnityEngine;

namespace Utils
{
    public readonly struct Line
    {
        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public Vector2 Start { get; }
        public Vector2 End { get; }

        public override string ToString() => $"({Start}, {End})";
    }

    public static class LineExtensions
    {
        public static float Cross(this Vector2 v1, Vector2 v2) => (v1.x * v2.y) - (v1.y * v2.x);

        public static bool Intersects(this Line line, Line other, out Vector2 intersection)
        {
            var p1 = line.Start;
            var p2 = line.End;
            var p3 = other.Start;
            var p4 = other.End;

            intersection = Vector2.zero;

            var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

            if (d == 0.0f)
                return false;

            var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
            var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

            if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
                return false;

            intersection.x = p1.x + u * (p2.x - p1.x);
            intersection.y = p1.y + u * (p2.y - p1.y);

            return true;
        }
    }
}