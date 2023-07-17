using System.Collections.Generic;
using Battlers;
using Grid;
using UnityEngine;
using UnityEngine.Events;

namespace Utils.Channels
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Channels/BattleChannel")]
    public class BattleChannel : ScriptableObject
    {
        public event UnityAction<Queue<BattlerInstance>> turnOrderResolvedEvent;
        public event UnityAction<BattlerInstance> currentBattlerChangedEvent;
        public event UnityAction<BattlerInstance> startedHoveringBattlerEvent;
        public event UnityAction<BattlerInstance> stoppedHoveringBattlerEvent;
        public event UnityAction<Skill> skillSelectedEvent;
        public event UnityAction<BattlerInstance, Skill, List<Node>, IEnumerable<BattlerInstance>> skillCastEvent;
        public event UnityAction<BattlerInstance> battlerFaintedEvent;
        public event UnityAction<BattlerInstance, Stat, int> statChangedEvent;

        public void RaiseTurnOrderResolved(Queue<BattlerInstance> battlersQueue) => turnOrderResolvedEvent?.Invoke(battlersQueue);
        public void RaiseCurrentBattlerChanged(BattlerInstance battler) => currentBattlerChangedEvent?.Invoke(battler);
        public void RaiseStartedHoveringBattler(BattlerInstance battler) => startedHoveringBattlerEvent?.Invoke(battler);
        public void RaiseStoppedHoveringBattler(BattlerInstance battler) => stoppedHoveringBattlerEvent?.Invoke(battler);
        public void RaiseSkillSelected(Skill skill) => skillSelectedEvent?.Invoke(skill);
        public void RaiseBattlerFainted(BattlerInstance battler) => battlerFaintedEvent?.Invoke(battler);
        public void RaiseStatChanged(BattlerInstance battler, Stat stat, int amount) => statChangedEvent?.Invoke(battler, stat, amount);

        public void RaiseSkillCast
        (
            BattlerInstance currentBattler, 
            Skill currentSkill, 
            List<Node> currentShape, 
            IEnumerable<BattlerInstance> aliveBattlers)
        {
            skillCastEvent?.Invoke(currentBattler, currentSkill, currentShape, aliveBattlers);
        }
    }
}