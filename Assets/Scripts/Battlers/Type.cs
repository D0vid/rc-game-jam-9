using System.Linq;
using UnityEngine;

namespace Battlers
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Type")]
    public class Type : ScriptableObject
    {
        public new string name;
        public Type[] resistances;
        public Type[] weaknesses;
        public Type[] immunities;

        public float GetDamageFactorAgainst(Type[] typing)
        {
            float type1Factor = GetDamageFactorAgainst(typing[0]);
            float type2Factor = typing.Length > 1 ? GetDamageFactorAgainst(typing[1]) : 1f;
            return type1Factor * type2Factor;
        }

        private float GetDamageFactorAgainst(Type other)
        {
            if (other.immunities.Contains(this))
                return 0f;
            if (other.resistances.Contains(this))
                return 0.5f;
            if (other.weaknesses.Contains(this))
                return 1.5f;
            return 1f;
        }
    }
}