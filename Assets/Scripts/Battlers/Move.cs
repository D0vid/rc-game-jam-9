using Array2DEditor;
using UnityEngine;

namespace Battlers
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Move")]
    public class Move : ScriptableObject
    {
        public new string name;
        [TextArea] public string description;
        public Sprite icon;
        public Animation animation;
        
        public Type type;
        public Category category;

        public Effect[] effects;
        public int damage;
        public int cost;
        public int cooldown;
        
        public int accuracy;
        public int criticalRate;

        public int range;
        public bool fixedRange;
        public bool selfCast;
        public bool lineRestricted;
        public bool lineOfSightRestricted;
        public Array2DInt shape;
    }

    public enum Category
    {
        Physical,
        Special
    }
}