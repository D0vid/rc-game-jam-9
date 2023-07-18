using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;
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

        public int CurrentAtk
        {
            get => _currentAtk;
            set => _currentAtk = value;
        }

        public int CurrentDef
        {
            get => _currentDef;
            set => _currentDef = value;
        }

        public int CurrentSpAtk
        {
            get => _currentSpAtk;
            set => _currentSpAtk = value;
        }

        public int CurrentSpDef
        {
            get => _currentSpDef;
            set => _currentSpDef = value;
        }

        public int MaxHp => battler.MaxHealth;
        public float PercentHp => CurrentHp / (float)MaxHp;
        public int CurrentHp
        {
            get => _currentHp;
            set
            {
                var amount = value - _currentHp;
                _currentHp = value < 0 ? 0 : value;
                battleChannel.RaiseStatChanged(this, Stat.Health, amount);
                StartCoroutine(AnimateHealthChange(amount));
                if (_currentHp == 0)
                {
                    StartCoroutine(Faint());
                }
            }
        }

        public int CurrentMp
        {
            get => _currentMp;
            set
            {
                var amount = value - _currentMp;
                _currentMp = value < 0 ? 0 : value;
                battleChannel.RaiseStatChanged(this, Stat.MovementPoints, amount);
            }
        }

        public int CurrentPp
        {
            get => _currentPp;
            set
            {
                var amount = value - _currentPp;
                _currentPp = value < 0 ? 0 : value;
                battleChannel.RaiseStatChanged(this, Stat.PowerPoints, amount);
            }
        }

        public int CurrentRange
        {
            get => _currentRange;
            set
            {
                var amount = value - _currentRange;
                _currentRange = value < 0 ? 0 : value;
                battleChannel.RaiseStatChanged(this, Stat.Range, amount);
            }
        }

        public List<Skill> Skills => battler.Skills.ToList();

        public BattlerState State { get; set; }

        private BattlerAnimator _animator;
        
        private int _currentHp;
        private int _currentMp;
        private int _currentPp;
        private int _currentSpDef;
        private int _currentSpAtk;
        private int _currentDef;
        private int _currentAtk;
        private int _currentRange;

        private void Awake()
        {
            _animator = GetComponent<BattlerAnimator>();
        }

        private void Start()
        {
            State = BattlerState.Idle;
            Destination = Position;
            _currentAtk = battler.Attack;
            _currentDef = battler.Defence;
            _currentSpAtk = battler.SpecialAtk;
            _currentSpDef = battler.SpecialDef;
            _currentMp = battler.MovementPoints;
            _currentPp = battler.PowerPoints;
            _currentRange = battler.Range;
            _currentHp = battler.MaxHealth;
        }

        public void OnPointerEnter(PointerEventData eventData) => battleChannel.RaiseStartedHoveringBattler(this);

        public void OnPointerExit(PointerEventData eventData) => battleChannel.RaiseStoppedHoveringBattler(this);

        public void ResetStats()
        {
            if (State != BattlerState.Fainted)
                State = BattlerState.Idle;
            
            _currentMp = battler.MovementPoints;
            _currentPp = battler.PowerPoints;
            _currentRange = battler.Range;
        }

        public IEnumerator FollowPath(List<Node> path, Action onEndOfPathReached)
        {
            State = BattlerState.Moving;
            Destination = path[^1].WorldPosition;
            foreach (Node node in path)
            {
                yield return StartCoroutine(Move(node));
            }
            _currentMp -= path.Count;
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
            if (currentSkill.cost <= CurrentPp)
            {
                _currentPp -= currentSkill.cost;
                if (targetPos != Position)
                    _animator.FaceTowards(targetPos);
                State = BattlerState.Attacking;
            }
        }

        private IEnumerator AnimateHealthChange(int amount)
        {
            yield return StartCoroutine(_animator.AnimateHealthChange(amount));
        }

        private IEnumerator Faint()
        {
            State = BattlerState.Fainted;
            battleChannel.RaiseBattlerFainted(this);
            yield return StartCoroutine(_animator.AnimateFaint());
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