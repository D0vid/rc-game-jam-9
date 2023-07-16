using System.Collections;
using UnityEngine;

namespace Utils
{
    public class SpriteAnimator
    {
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Sprite[] _frames;
        private readonly float _frameRate;

        private int _currentFrame;
        private float _timer;
        private int _loops;

        public SpriteAnimator(SpriteRenderer spriteRenderer, Sprite[] frames, float frameRate = 0.16f)
        {
            _spriteRenderer = spriteRenderer;
            _frames = frames;
            _frameRate = frameRate;
        }

        public void Reset()
        {
            _currentFrame = 0;
            _timer = 0f;
            _spriteRenderer.sprite = _frames[0];
            _loops = 0;
        }

        public void Play()
        {
            if (_frames == null)
                return;

            _timer += Time.deltaTime;
            if (_timer > _frameRate)
            {
                _currentFrame = (_currentFrame + 1) % _frames.Length;
                _spriteRenderer.sprite = _frames[_currentFrame];
                _timer -= _frameRate;
                _loops++;
            }
        }

        public bool IsPlayingLastFrame() => _currentFrame == 0 && _loops != 0;
    }
}