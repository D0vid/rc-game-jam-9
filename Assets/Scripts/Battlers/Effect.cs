using System;
using UnityEngine;

namespace Battlers
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Effect")]
    public class Effect : ScriptableObject
    {
        public new string name;
        [TextArea] public string description;
        public Sprite icon;

        public int chanceToApply;
        public Stat stat;
        public Operation operation;
        public int duration;
        public float value;
        public Condition[] condition;
        
        public TriggerPhase triggerPhase;
    }
    
    [Serializable]
    public class Condition
    {
        [field: SerializeField] public Stat Stat { get; private set; }
        [field: SerializeField] public ConditionalOperator Operator { get; private set; }
        [field: SerializeField] public ConditionalValueType ConditionalValueType { get; private set; }
        [field: SerializeField] public int Value { get; private set; }
    }

    public enum ConditionalOperator
    {
        GreaterThan,
        LesserThan,
        EqualTo
    }

    public enum ConditionalValueType
    {
        Percentage,
        Fixed
    }

    public enum Stat
    {
        Health,
        Attack,
        Defense,
        SpAttack,
        SpDefense,
        MovementPoints,
        PowerPoints,
        Range
    }

    public enum Operation
    {
        Additive,
        Multiplicative
    }

    public enum TriggerPhase
    {
        BeginningOfTurn,
        EndOfTurn
    }
}