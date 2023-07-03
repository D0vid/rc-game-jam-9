using System;
using UnityEngine;

namespace Battlers
{
    [Serializable]
    public class Battler
    {
        [SerializeField] private int level;
        [SerializeField] private BattlerBase battlerBase;

        private readonly Guid _id;
        
        public Battler(BattlerBase battlerBase, int level)
        {
            _id = new Guid();
            this.battlerBase = battlerBase;
            this.level = level;
        }

        public string Id => _id.ToString();
        
        public int DexNumber => battlerBase.dexNumber;
        public string Name => battlerBase.name;
        public Sprite Sprite => battlerBase.sprite;

        public Type[] Typing => battlerBase.typing;

        public int MaxHealth => BaseStatCalculation(battlerBase.health) + 5;
        public int Attack => BaseStatCalculation(battlerBase.attack);
        public int Defence => BaseStatCalculation(battlerBase.defence);
        public int SpecialAtk => BaseStatCalculation(battlerBase.specialAtk);
        public int SpecialDef => BaseStatCalculation(battlerBase.specialDef);
        public int Speed => BaseStatCalculation(battlerBase.speed);

        public Move[] MoveSet => battlerBase.moveSet;
        public Effect Ability => battlerBase.ability;

        public int PowerPoints => battlerBase.powerPoints;
        public int MovementPoints => battlerBase.movementPoints;
        public int Range => battlerBase.range;

        public int Level { get; set; }

        public override string ToString()
        {
            return $"{Name} Lv.{Level}";
        }

        protected bool Equals(Battler other)
        {
            return _id.Equals(other._id);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        private int BaseStatCalculation(int baseValue) => Mathf.FloorToInt(battlerBase.health * level / 100f);
    }
}