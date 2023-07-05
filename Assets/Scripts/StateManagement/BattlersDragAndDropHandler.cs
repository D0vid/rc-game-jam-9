using System.Collections.Generic;
using Battlers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace StateManagement
{
    public class BattlersDragAndDropHandler
    {
        private readonly List<BattlerInstance> _party;
        private readonly List<Vector2> _partyPlacements;
        private readonly Tilemap _walkableTilemap;
        
        private Vector3 _draggedBattlerOriginalPosition;
        private Transform _draggedBattlerTransform;

        public BattlersDragAndDropHandler(List<BattlerInstance> party, List<Vector2> partyPlacements, Tilemap walkableTilemap)
        {
            _party = party;
            _partyPlacements = partyPlacements;
            _walkableTilemap = walkableTilemap;
        }

        public void OnBeginMouseDrag(Vector2 mousePos)
        {
            var cellPos = _walkableTilemap.WorldToCell(mousePos);
            if (_partyPlacements.Contains(_walkableTilemap.CellToWorld(cellPos)))
            {
                Vector2 snappedToGrid = _walkableTilemap.CellToWorld(cellPos);
                BattlerInstance battler = _party.Find(ally => ally.transform.position.Equals(snappedToGrid));
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
            var cellPos = _walkableTilemap.WorldToCell(mousePos);
            var worldPosCentered = _walkableTilemap.CellToWorld(cellPos);
            if (_draggedBattlerTransform != null && _walkableTilemap.HasTile(cellPos))
            {
                _draggedBattlerTransform.position = worldPosCentered;
            }
        }

        public void OnEndMouseDrag(Vector2 mousePos)
        {
            var cellPos = _walkableTilemap.WorldToCell(mousePos);
            var worldPosCentered = _walkableTilemap.CellToWorld(cellPos);
            if (_draggedBattlerTransform != null)
            {
                // Reset battler position on invalid placement
                if (!_walkableTilemap.HasTile(cellPos) || !_partyPlacements.Contains(worldPosCentered))
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
            BattlerInstance teammate = _party.Find(ally =>
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