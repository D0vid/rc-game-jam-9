using System.Collections;
using System.Linq;
using UnityEngine;
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
            }

            if (_currentAnimator != previousAnimator)
                _currentAnimator.Reset();

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
            Debug.Log(position);
            _lastDirectionIndex = GetDirectionIndex(direction);
            Debug.Log(_lastDirectionIndex);
        }

        public void SetAnimationSprites(Sprite[] battlerSprites)
        {
            var idleNorthWest = battlerSprites.Skip(0).Take(4).ToArray();
            var idleSouthWest = battlerSprites.Skip(4).Take(4).ToArray();
            var idleSouthEast = battlerSprites.Skip(8).Take(4).ToArray();
            var idleNorthEast = battlerSprites.Skip(12).Take(4).ToArray();
            var walkNorthWest = battlerSprites.Skip(16).Take(4).ToArray();
            var walkSouthWest = battlerSprites.Skip(20).Take(4).ToArray();
            var walkSouthEast = battlerSprites.Skip(24).Take(4).ToArray();
            var walkNorthEast = battlerSprites.Skip(28).Take(4).ToArray();
            var attackNorthWest = battlerSprites.Skip(16).Take(4).ToArray();
            var attackSouthWest = battlerSprites.Skip(20).Take(4).ToArray();
            var attackSouthEast = battlerSprites.Skip(24).Take(4).ToArray();
            var attackNorthEast = battlerSprites.Skip(28).Take(4).ToArray();
            
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
    }
}