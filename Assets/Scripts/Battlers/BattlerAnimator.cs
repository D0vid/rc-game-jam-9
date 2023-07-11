using System.Linq;
using UnityEngine;
using Utils;

namespace Battlers
{
    public class BattlerAnimator : MonoBehaviour
    {
        public Sprite[] idleNorthEast;
        public Sprite[] idleSouthEast;
        public Sprite[] idleSouthWest;
        public Sprite[] idleNorthWest;

        public Sprite[] walkNorthEast;
        public Sprite[] walkSouthEast;
        public Sprite[] walkSouthWest;
        public Sprite[] walkNorthWest;

        private SpriteAnimator[] _idleAnimators;
        private SpriteAnimator[] _walkAnimators;

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

            if (_battlerInstance.State == BattlerState.Idle)
            {
                _currentAnimator = _idleAnimators[_lastDirectionIndex];
            }
            else if (_battlerInstance.State == BattlerState.Moving && direction.sqrMagnitude > Mathf.Epsilon)
            {
                _lastDirectionIndex = GetDirectionIndex(direction);
                _currentAnimator = _walkAnimators[_lastDirectionIndex];
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

        public void SetAnimationSprites(Sprite[] battlerSprites)
        {
            idleNorthWest = battlerSprites.Skip(0).Take(4).ToArray();
            idleSouthWest = battlerSprites.Skip(4).Take(4).ToArray();
            idleSouthEast = battlerSprites.Skip(8).Take(4).ToArray();
            idleNorthEast = battlerSprites.Skip(12).Take(4).ToArray();
            walkNorthWest = battlerSprites.Skip(16).Take(4).ToArray();
            walkSouthWest = battlerSprites.Skip(20).Take(4).ToArray();
            walkSouthEast = battlerSprites.Skip(24).Take(4).ToArray();
            walkNorthEast = battlerSprites.Skip(28).Take(4).ToArray();
            
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
        }
    }
}