using System;
using UnityEngine;
using Zenject;

public class DesktopInput : ITickable, IInput {
    public event Action SwipeDown;
    public event Action SwipeUp;
    public event Action SwipeRight;
    public event Action SwipeLeft;

    private const int LeftMouseButton = 0;

    private bool _isSwiping;

    private Vector2 _swipeDelta;
    private float _deadZone = 80;
    private Vector2 _startPosition;

    public void Tick() {

        ProcessClickUp();

        ProcessClickDown();

        ProcessSwipe();
    }

    private void ProcessSwipe() {
        if (_isSwiping == false)
            return;

        CheckSwipe();
    }

    private void ProcessClickDown() {
        if (Input.GetMouseButtonDown(LeftMouseButton)) {
            _isSwiping = true;
            _startPosition = Input.mousePosition;
        }
    }

    private void ProcessClickUp() {
        if (Input.GetMouseButtonUp(LeftMouseButton)) 
            _isSwiping = false;
    }

    private void CheckSwipe() {
        _swipeDelta = Vector2.zero;

        _swipeDelta = (Vector2)Input.mousePosition - (Vector2)_startPosition;

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

        _startPosition =  Vector2.zero;
        _swipeDelta = Vector2.zero;
    }
}
