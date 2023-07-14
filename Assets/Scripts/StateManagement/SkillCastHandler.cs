using System.Collections.Generic;
using Battlers;
using Grid;
using UnityEngine;

namespace StateManagement
{
    public class SkillCastHandler
    {
        private readonly BattleManager _battleManager;

        private List<Node> _currentTargetableNodes;
        private Skill _currentSkill;

        public SkillCastHandler(BattleManager battleManager)
        {
            _battleManager = battleManager;
        }

        public void HandleSkillSelected(Skill skill)
        {
            if (_battleManager.CurrentBattler.CurrentPP < skill.cost)
            {
                Debug.Log("Not enough PP for this move.");
                return;
            }
            _battleManager.CurrentBattler.State = BattlerState.Casting;
            Vector2 battlerPos = _battleManager.CurrentBattler.Position; // TODO change to destination to speed up battle
            var nodesInRange = DetermineNodesInRange(skill, battlerPos);
            _currentTargetableNodes = skill.lineOfSightRestricted
                ? _battleManager.FilterTargetableNodes(battlerPos, nodesInRange)
                : nodesInRange;
            _battleManager.HighlightSkillRange(nodesInRange, _currentTargetableNodes);
            _currentSkill = skill;
        }

        private List<Node> DetermineNodesInRange(Skill skill, Vector2 battlerPos)
        {
            var nodesInRange = _battleManager.GetNodesInArea(battlerPos, skill.range, skill.lineRestricted);
            if (!skill.selfCast)
                nodesInRange.Remove(_battleManager.GetNodeForWorldPos(battlerPos));
            return nodesInRange;
        }
    }
}