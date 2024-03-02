using System;
using UnityEngine;

public class AnimatorEventsHandler : MonoBehaviour {
    public event Action JumpStarted;
    public event Action JumpProgressed;
    public event Action Jumped;

    public event Action StrikStarted;
    public event Action Striked;

    public void JumpStart() => JumpStarted?.Invoke();
    
    public void JumpProgress() => JumpProgressed?.Invoke();
    
    public void JumpComplited() => Jumped?.Invoke();
    
    public void StrikeStart() => StrikStarted?.Invoke();
    
    public void StrikeComplited() => Striked?.Invoke();
}
