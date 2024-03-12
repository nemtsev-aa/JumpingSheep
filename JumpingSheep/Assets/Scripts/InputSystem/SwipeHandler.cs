using System;
using UnityEngine;

public enum SwipeDirection {
    None,
    Up,
    Down,
    Right,
    Left
}

public class SwipeHandler: IPause, IDisposable {
    public event Action<SwipeDirection> SwipeDirectionChanged;
    private readonly IInput _input;
    private bool _isPaused;

    public SwipeHandler(IInput input) {
        _input = input;

        _input.SwipeUp += OnSwipeUp;
        _input.SwipeDown += OnSwipeDown;
        _input.SwipeLeft += OnSwipeLeft;
        _input.SwipeRight += OnSwipeRight;
    }

    public void SetPause(bool isPaused) => _isPaused = isPaused;

    public void Dispose() {
        _input.SwipeUp -= OnSwipeUp;
        _input.SwipeDown -= OnSwipeDown;
        _input.SwipeLeft -= OnSwipeLeft;
        _input.SwipeRight -= OnSwipeRight;
    }

    private void OnSwipeUp() => SwipeDirectionChanged?.Invoke(SwipeDirection.Up);

    private void OnSwipeDown() => SwipeDirectionChanged?.Invoke(SwipeDirection.Down);

    private void OnSwipeLeft() => SwipeDirectionChanged?.Invoke(SwipeDirection.Left);
 
    private void OnSwipeRight() => SwipeDirectionChanged?.Invoke(SwipeDirection.Right);

}
