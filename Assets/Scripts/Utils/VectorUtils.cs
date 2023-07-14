using UnityEngine;

namespace Utils
{
    public static class VectorUtils
    {
        public static Vector2 AddOffSet(this Vector2 vector) => vector + new Vector2(0, 0.25f);

        public static Vector3 AddOffSet(this Vector3 vector) => vector + new Vector3(0, 0.25f, 0);
    }
}