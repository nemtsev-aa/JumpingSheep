using System;
using UnityEngine;

public enum SwipeDirection {
    None,
    Up,
    Down,
    Right,
    Left
}

public class SwipeHandler: IDisposable {
    public event Action<SwipeDirection> SwipeDirectionChanged;
    private IInput _input;

    public SwipeHandler(IInput input) {
        _input = input;

        _input.SwipeUp += OnSwipeUp;
        _input.SwipeDown += OnSwipeDown;
        _input.SwipeLeft += OnSwipeLeft;
        _input.SwipeRight += OnSwipeRight;
    }

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
