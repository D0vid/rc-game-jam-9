using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlers
{
    public class BattlerInstance : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Battler battler;
        public float moveSpeed = 3.5f;
        
        public Team Team { get; set; }
        public Vector2 Position => transform.position;

        public int CurrentAtk { get; set; }
        public int CurrentDef { get; set; }
        public int CurrentSpAtk { get; set; }
        public int CurrentSpDef { get; set; }
        public int CurrentHP { get; set; }
        public int MaxHP => battler.MaxHealth;
        public int PercentHP => CurrentHP / MaxHP;
        public int CurrentMP { get; set; }
        public int CurrentPP { get; set; }
        public int CurrentRange { get; set; }
        
        public BattlerState State { get; set; }

        public bool IsHovered { get; set; }

        private BattlerAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<BattlerAnimator>();
        }

        private void Start()
        {
            State = BattlerState.Idle;
            CurrentAtk = battler.Attack;
            CurrentDef = battler.Defence;
            CurrentSpAtk = battler.SpecialAtk;
            CurrentSpDef = battler.SpecialDef;
            CurrentMP = battler.MovementPoints;
            CurrentPP = battler.PowerPoints;
            CurrentRange = battler.Range;
        }

        public void OnPointerEnter(PointerEventData eventData) => IsHovered = true;

        public void OnPointerExit(PointerEventData eventData) => IsHovered = false;

        public void ResetStats()
        {
            State = BattlerState.Idle;
            CurrentMP = battler.MovementPoints;
            CurrentPP = battler.PowerPoints;
            CurrentRange = battler.Range;
        }

        public IEnumerator FollowPath(List<Node> path, Action onEndOfPathReached)
        {
            State = BattlerState.Moving;
            foreach (Node node in path)
            {
                yield return StartCoroutine(Move(node));
            }
            CurrentMP -= path.Count;
            State = BattlerState.Idle;
            onEndOfPathReached();
        }

        private IEnumerator Move(Node destinationNode)
        {
            Vector3 destination = destinationNode.WorldPosition;
            _animator.TargetPos = destination;
            while (transform.position != destination)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public enum BattlerState
    {
        Idle,
        Moving,
        Casting,
    }

    public enum Team
    {
        Party,
        Enemies
    }
}