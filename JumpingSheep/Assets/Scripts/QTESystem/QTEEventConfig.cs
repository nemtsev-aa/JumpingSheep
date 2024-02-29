using UnityEngine;
using System;

[Serializable]
public class QTEEventConfig {
    [field: SerializeField] public string KeyText { get; private set; }
    [field: SerializeField] public KeyCode KeyKode { get; private set; }
    [field: SerializeField] public float KeyTime { get; private set; }
}
