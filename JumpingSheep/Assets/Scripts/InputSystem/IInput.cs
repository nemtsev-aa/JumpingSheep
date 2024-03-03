using System;
using UnityEngine;

public interface IInput {
    event Action SwipeDown;
    event Action SwipeUp;
    event Action SwipeRight;
    event Action SwipeLeft;
}
