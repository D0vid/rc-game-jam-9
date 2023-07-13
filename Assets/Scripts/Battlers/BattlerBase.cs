using UnityEngine;
using UnityEngine.Serialization;

namespace Battlers
{
    [CreateAssetMenu(menuName = "ScriptableObjects/BattlerBase")]
    public class BattlerBase : ScriptableObject
    {
        public int dexNumber;
        
        public Sprite[] sprites;
        public Sprite timelineSprite;
        public Sprite portrait;

        public Type[] typing;
        
        public int health = 100;
        public int attack = 100;
        public int defence = 100;
        public int specialAtk = 100;
        public int specialDef = 100;
        public int speed = 100;

        public Move[] moveSet;
        public Effect ability;
        
        public int powerPoints = 6;
        public int movementPoints = 3;
        public int range = 0;
    }
}