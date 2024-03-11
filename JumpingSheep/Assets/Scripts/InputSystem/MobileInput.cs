using System;
using UnityEngine;
using Zenject;

public class MobileInput : ITickable, IInput {
    public event Action SwipeDown;
    public event Action SwipeUp;
    public event Action SwipeRight;
    public event Action SwipeLeft;

    private const int FirstTouch = 0;

    private Vector2 _startPosition;

    private Vector2 _swipeDelta;
    private bool _isSwiping;
    private float _deadZone = 80;

    public void Tick() {
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(FirstTouch);

            switch (touch.phase) {
                case TouchPhase.Began:

                    _isSwiping = true;
                    _startPosition = touch.position;

                    break;

                case TouchPhase.Moved:
                    break;

                case TouchPhase.Ended:
                    ResetSwipe();
                    break;
            }
        }

        CheckSwipe();
    }

    private void CheckSwipe() {
        _swipeDelta = Vector2.zero;

        if (_isSwiping && Input.touchCount > 0)
            _swipeDelta = Input.GetTouch(FirstTouch).position - _startPosition;

        if (_swipeDelta.magnitude > _deadZone) {
            
            if (Mathf.Abs(_swipeDelta.x) > Mathf.Abs(_swipeDelta.y)) {
                if (_swipeDelta.x > 0) 
                    SwipeRight?.Invoke();
                else
                    SwipeLeft?.Invoke();
            }
            else 
            {
                if (_swipeDelta.y > 0)
                    SwipeUp?.Invoke();
                else
                    SwipeDown?.Invoke();
            }

            ResetSwipe();
        }
    }

    private void ResetSwipe() {
        _isSwiping = false;

        _startPosition = Vector2.zero;
        _swipeDelta = Vector2.zero;
    }
}
