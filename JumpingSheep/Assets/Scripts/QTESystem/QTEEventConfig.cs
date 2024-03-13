using UnityEngine;
using System;

[Serializable]
public class QTEEventConfig {
    [field: SerializeField] public Sprite SwipeDirectionIcon { get; private set; }
    [field: SerializeField] public SwipeDirection SwipeDirectionValue { get; private set; }
    public float TimeToSwipe { get; set; }
}
