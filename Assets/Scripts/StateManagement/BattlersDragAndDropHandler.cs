using System.Collections.Generic;
using Battlers;
using Grid;
using UnityEngine;

namespace StateManagement
{
    public class BattlersDragAndDropHandler
    {
        private readonly BattleGrid _battleGrid;

        private Vector3 _draggedBattlerOriginalPosition;
        private Transform _draggedBattlerTransform;
        
        public List<BattlerInstance> Party { get; set; }

        public BattlersDragAndDropHandler(BattleGrid battleGrid)
        {
            _battleGrid = battleGrid;
        }

        public void OnBeginMouseDrag(Vector2 mousePos)
        {
            mousePos = _battleGrid.SnapPositionToGrid(mousePos);
            if (_battleGrid.PartyPlacements.Contains(mousePos))
            {
                BattlerInstance battler = Party.Find(ally => ally.transform.position.Equals(mousePos));
                if (battler != null)
                {
                    _draggedBattlerTransform = battler.transform;
                    _draggedBattlerOriginalPosition = _draggedBattlerTransform.position;
                    battler.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
                }
            }
        }

        public void OnMouseDrag(Vector2 mousePos)
        {
            mousePos = _battleGrid.SnapPositionToGrid(mousePos);
            if (_draggedBattlerTransform != null && _battleGrid.IsWalkable(mousePos))
            {
                _draggedBattlerTransform.position = mousePos;
            }
        }

        public void OnEndMouseDrag(Vector2 mousePos)
        {
            mousePos = _battleGrid.SnapPositionToGrid(mousePos);
            if (_draggedBattlerTransform != null)
            {
                // Reset battler position on invalid placement
                if (!_battleGrid.IsWalkable(mousePos) || !_battleGrid.PartyPlacements.Contains(mousePos))
                    _draggedBattlerTransform.position = _draggedBattlerOriginalPosition; 
                // Swap battlers if necessary
                Transform eventualTeammateToSwapTransform = FindEventualTeammateToSwapTransform();
                if (eventualTeammateToSwapTransform != null)
                    SwapPositions(_draggedBattlerTransform, eventualTeammateToSwapTransform);

                ResetDraggedBattler();
            }
        }

        private Transform FindEventualTeammateToSwapTransform()
        {
            BattlerInstance teammate = Party.Find(ally =>
            {
                bool isDifferentBattler = ally.gameObject != _draggedBattlerTransform.gameObject;
                bool isAtSamePosition = ally.transform.position.Equals(_draggedBattlerTransform.position);
                return isDifferentBattler && isAtSamePosition;
            });
            return teammate != null ? teammate.transform : null;
        }

        private void SwapPositions(Transform draggedBattlerTransform, Transform otherBattlerTransform)
        {
            draggedBattlerTransform.position = otherBattlerTransform.position;
            otherBattlerTransform.position = _draggedBattlerOriginalPosition;
        }

        private void ResetDraggedBattler()
        {
            _draggedBattlerTransform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            _draggedBattlerTransform = null;
            _draggedBattlerOriginalPosition = Vector2.zero;
        }
    }
}