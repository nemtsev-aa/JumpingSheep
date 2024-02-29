using System;
using UnityEngine;

public class AnimatorEventsHandler : MonoBehaviour {
    public event Action Striked;
    public event Action Jumped;


    public void JumpComplited() {
        Jumped?.Invoke();
    }

    public void StrikeComplited() {
        Striked?.Invoke();
    }
}
