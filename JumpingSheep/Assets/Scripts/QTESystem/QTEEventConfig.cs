using UnityEngine;
using System;

[Serializable]
public class QTEEventConfig {
    [field: SerializeField] public Sprite KeyIcon { get; private set; }
    [field: SerializeField] public string KeyText { get; private set; }
    [field: SerializeField] public KeyCode KeyKode { get; private set; }
    [field: SerializeField] public SwipeDirection SwipeDirection { get; private set; }
    [field: SerializeField] public float KeyTime { get; set; }
}
