using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Utils;

namespace Battlers
{
    public class BattlerAnimator : MonoBehaviour
    {
        private SpriteAnimator[] _idleAnimators;
        private SpriteAnimator[] _walkAnimators;
        private SpriteAnimator[] _attackAnimators;

        private SpriteRenderer _spriteRenderer;
        private BattlerInstance _battlerInstance;
        private SpriteAnimator _currentAnimator;

        private int _lastDirectionIndex;

        public Vector2 TargetPos { get; set; }

        private void Awake()
        {
            _battlerInstance = GetComponent<BattlerInstance>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _lastDirectionIndex = 1;
            _currentAnimator = _idleAnimators[_lastDirectionIndex];
        }

        private void Update()
        {
            var previousAnimator = _currentAnimator;
            Vector2 direction = TargetPos - _battlerInstance.Position;

            switch (_battlerInstance.State)
            {
                case BattlerState.Idle or BattlerState.Casting:
                    _currentAnimator = _idleAnimators[_lastDirectionIndex];
                    break;
                case BattlerState.Moving when direction.sqrMagnitude > Mathf.Epsilon:
                    _lastDirectionIndex = GetDirectionIndex(direction);
                    _currentAnimator = _walkAnimators[_lastDirectionIndex];
                    break;
                case BattlerState.Attacking:
                    _currentAnimator = _attackAnimators[_lastDirectionIndex];
                    break;
            }

            if (_currentAnimator != previousAnimator)
                _currentAnimator.Reset();

            if (_attackAnimators.Contains(_currentAnimator) && _currentAnimator.IsPlayingLastFrame())
                _battlerInstance.State = BattlerState.Idle;

            _currentAnimator.Play();
        }

        private int GetDirectionIndex(Vector2 direction)
        {
            float step = 360 / 8f;
            float offset = step / 2;
            float angle = Vector2.SignedAngle(Vector2.up, direction);

            angle += offset;

            if (angle < 0) angle += 360;

            float stepCount = angle / step;

            return Mathf.FloorToInt(stepCount / 2);
        }

        public void FaceTowards(Vector2 position)
        {
            Vector2 direction = position - _battlerInstance.Position;
            _lastDirectionIndex = GetDirectionIndex(direction);
        }

        public void SetAnimationSprites(Sprite[] idleAnims, Sprite[] walkAnims, Sprite[] attackAnims)
        {
            var idleNorthWest = idleAnims.Skip(0).Take(4).ToArray();
            var idleSouthWest = idleAnims.Skip(4).Take(4).ToArray();
            var idleSouthEast = idleAnims.Skip(8).Take(4).ToArray();
            var idleNorthEast = idleAnims.Skip(12).Take(4).ToArray();
            
            var walkNorthWest = walkAnims.Skip(0).Take(4).ToArray();
            var walkSouthWest = walkAnims.Skip(4).Take(4).ToArray();
            var walkSouthEast = walkAnims.Skip(8).Take(4).ToArray();
            var walkNorthEast = walkAnims.Skip(12).Take(4).ToArray();
            
            var attackNorthWest = attackAnims.Skip(0).Take(4).ToArray();
            var attackSouthWest = attackAnims.Skip(4).Take(4).ToArray();
            var attackSouthEast = attackAnims.Skip(8).Take(4).ToArray();
            var attackNorthEast = attackAnims.Skip(12).Take(4).ToArray();
            
            _idleAnimators = new[]
            {
                new SpriteAnimator(_spriteRenderer, idleNorthWest),
                new SpriteAnimator(_spriteRenderer, idleSouthWest),
                new SpriteAnimator(_spriteRenderer, idleSouthEast),
                new SpriteAnimator(_spriteRenderer, idleNorthEast),
            };
            
            _walkAnimators = new[]
            {
                new SpriteAnimator(_spriteRenderer, walkNorthWest),
                new SpriteAnimator(_spriteRenderer, walkSouthWest),
                new SpriteAnimator(_spriteRenderer, walkSouthEast),
                new SpriteAnimator(_spriteRenderer, walkNorthEast),
            };
            
            _attackAnimators = new[]
            {
                new SpriteAnimator(_spriteRenderer, attackNorthWest),
                new SpriteAnimator(_spriteRenderer, attackSouthWest),
                new SpriteAnimator(_spriteRenderer, attackSouthEast),
                new SpriteAnimator(_spriteRenderer, attackNorthEast),
            };
        }

        public IEnumerator AnimateHealthChange(int amount)
        {
            Color targetColor = amount < 0 ? Color.red : Color.green;
            Color originColor = Color.white;

            Sequence seq = DOTween.Sequence();
            seq.Append(_spriteRenderer.DOColor(targetColor, 0.5f / 2));
            seq.Append(_spriteRenderer.DOColor(originColor, 0.5f / 2)); 

            yield return seq.WaitForCompletion();
        }

        public IEnumerator AnimateFaint()
        {
            Color targetColor = Color.red;

            Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f);

            Sequence seq = DOTween.Sequence();
            seq.Append(_spriteRenderer.DOColor(targetColor, 0.5f / 4));
            seq.Append(transform.DOScale(targetScale, 0.5f * 3 / 4));

            yield return seq.WaitForCompletion();
        }
    }
}