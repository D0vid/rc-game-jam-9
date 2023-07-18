using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Utils.Channels;

namespace Battlers
{
    public class BattlerInstance : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public BattleChannel battleChannel;
        public Battler battler;
        public float moveSpeed = 3.5f;
        
        public Team Team { get; set; }
        
        public Vector2 Position => transform.position;
        public Vector2 Destination { get; set; }

        public int CurrentAtk { get; set; }
        public int CurrentDef { get; set; }
        public int CurrentSpAtk { get; set; }
        public int CurrentSpDef { get; set; }

        public int CurrentHP
        {
            get => _currentHp;
            private set
            {
                var amount = _currentHp - value;
                _currentHp = value < 0 ? 0 : value;
                battleChannel.RaiseStatChanged(this, Stat.Health, amount);
                if (_currentHp == 0)
                {
                    Faint();
                }
            }
        }

        public int MaxHP => battler.MaxHealth;
        public float PercentHP => CurrentHP / (float)MaxHP;
        public int CurrentMP { get; set; }
        public int CurrentPP { get; set; }
        public int CurrentRange { get; set; }
        public List<Skill> Skills => battler.Skills.ToList();

        public BattlerState State { get; set; }

        private BattlerAnimator _animator;
        private int _currentHp;

        private void Awake()
        {
            _animator = GetComponent<BattlerAnimator>();
        }

        private void Start()
        {
            State = BattlerState.Idle;
            Destination = Position;
            CurrentAtk = battler.Attack;
            CurrentDef = battler.Defence;
            CurrentSpAtk = battler.SpecialAtk;
            CurrentSpDef = battler.SpecialDef;
            CurrentMP = battler.MovementPoints;
            CurrentPP = battler.PowerPoints;
            CurrentRange = battler.Range;
            _currentHp = battler.MaxHealth;
        }

        public void OnPointerEnter(PointerEventData eventData) => battleChannel.RaiseStartedHoveringBattler(this);

        public void OnPointerExit(PointerEventData eventData) => battleChannel.RaiseStoppedHoveringBattler(this);

        public void ResetStats()
        {
            if (State != BattlerState.Fainted)
                State = BattlerState.Idle;
            
            CurrentMP = battler.MovementPoints;
            CurrentPP = battler.PowerPoints;
            CurrentRange = battler.Range;
        }

        public IEnumerator FollowPath(List<Node> path, Action onEndOfPathReached)
        {
            State = BattlerState.Moving;
            Destination = path[^1].WorldPosition;
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
            Vector3 step = destinationNode.WorldPosition;
            _animator.TargetPos = step;
            while (transform.position != step)
            {
                transform.position = Vector3.MoveTowards(transform.position, step, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        public void Cast(Skill currentSkill, Vector2 targetPos)
        {
            if (currentSkill.cost <= CurrentPP)
            {
                CurrentPP -= currentSkill.cost;
                if (targetPos != Position)
                    _animator.FaceTowards(targetPos);
                State = BattlerState.Attacking;
            }
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
        }

        private void Faint()
        {
            State = BattlerState.Fainted;
            battleChannel.RaiseBattlerFainted(this);
            StartCoroutine(FaintCoroutine()); // TODO animation first
        }

        private IEnumerator FaintCoroutine()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }

    public enum BattlerState
    {
        Idle,
        Moving,
        Casting,
        Attacking,
        Fainted
    }

    public enum Team
    {
        Party,
        Enemies
    }
}