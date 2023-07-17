using System.Collections.Generic;
using Battlers;
using Grid;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Channels;

namespace StateManagement
{
    public class SkillCastHandler
    {
        private readonly BattleManager _battleManager;
        private readonly SkillShapeParser _skillShapeParser;
        private readonly Camera _mainCamera;
        private readonly BattleChannel _battleChannel;

        private List<Node> _currentTargetableNodes; // TODO remove Node references and use Vector2 instead
        private Skill _currentSkill;
        private List<Node> _currentShape;

        public SkillCastHandler(BattleManager battleManager)
        {
            _battleChannel = Resources.Load<BattleChannel>("Channels/BattleChannel");
            _battleManager = battleManager;
            _currentTargetableNodes = new List<Node>();
            _currentShape = new List<Node>();
            _skillShapeParser = new SkillShapeParser(_battleManager);
            _mainCamera = Camera.main;
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
            OnMouseHover(_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }

        private List<Node> DetermineNodesInRange(Skill skill, Vector2 battlerPos)
        {
            var nodesInRange = _battleManager.GetNodesInArea(battlerPos, skill.range, skill.lineRestricted);
            if (!skill.selfCast)
                nodesInRange.Remove(_battleManager.GetNodeForWorldPos(battlerPos));
            return nodesInRange;
        }

        public void OnMouseHover(Vector2 mousePos)
        {
            if (_battleManager.CurrentBattler.State == BattlerState.Casting)
            {
                _battleManager.RemoveSkillShapeHighlight();
                if (IsTargetable(mousePos))
                {
                    _currentShape = _skillShapeParser.ParseSkill(_currentSkill, mousePos);
                    _battleManager.HighlightSkillShape(_currentShape);
                }
            }
        }

        public void OnMouseClick(Vector2 mousePos)
        {
            if (IsTargetable(mousePos) && _battleManager.CurrentBattler.State == BattlerState.Casting)
            {
                var mousePosSnapped = _battleManager.SnapPositionToGrid(mousePos);
                _battleManager.CurrentBattler.Cast(_currentSkill, mousePosSnapped);
                _battleChannel.RaiseSkillCast(_battleManager.CurrentBattler, _currentSkill, _currentShape, _battleManager.AliveBattlers);
            }
            StopCasting();
        }

        private void StopCasting()
        {
            if (_battleManager.CurrentBattler.State == BattlerState.Casting)
            {
                _battleManager.CurrentBattler.State = BattlerState.Idle;
            }
            _battleManager.RemoveAllHighlights();
        }

        private bool IsTargetable(Vector2 position)
        {
            var node = _battleManager.GetNodeForWorldPos(position);
            return _currentTargetableNodes.Contains(node);
        }
    }
}