using System;
using UnityEngine;

namespace Battlers
{
    [Serializable]
    public class Battler
    {
        [SerializeField] private int level;
        [SerializeField] private BattlerBase battlerBase;

        public Battler(BattlerBase battlerBase, int level)
        {
            this.battlerBase = battlerBase;
            this.level = level;
        }
        
        public int DexNumber => battlerBase.dexNumber;
        public string Name => battlerBase.name;
        
        public Sprite[] IdleSprites => battlerBase.idleSprites;
        public Sprite[] WalkSprites => battlerBase.walkSprites;
        public Sprite[] AttackSprites => battlerBase.attackSprites;
        public Sprite TimelineSprite => battlerBase.timelineSprite;
        public Sprite Portrait => battlerBase.portrait;

        public Type[] Typing => battlerBase.typing;

        public int MaxHealth => BaseStatCalculation(battlerBase.health) + 5;
        public int Attack => BaseStatCalculation(battlerBase.attack);
        public int Defence => BaseStatCalculation(battlerBase.defence);
        public int SpecialAtk => BaseStatCalculation(battlerBase.specialAtk);
        public int SpecialDef => BaseStatCalculation(battlerBase.specialDef);
        public int Speed => BaseStatCalculation(battlerBase.speed);

        public Skill[] Skills => battlerBase.skills;
        public Effect Ability => battlerBase.ability;

        public int PowerPoints => battlerBase.powerPoints;
        public int MovementPoints => battlerBase.movementPoints;
        public int Range => battlerBase.range;

        public int Level { get; set; }

        public override string ToString()
        {
            return $"{Name} Lv.{Level}";
        }

        private int BaseStatCalculation(int baseValue)
        {
            return Mathf.FloorToInt(((2 * baseValue) + 31 + 255) * (level / 100f));
        }
    }
}