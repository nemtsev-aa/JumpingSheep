using UnityEngine;
using System;

[Serializable]
public class QTESystemConfig {
    [field: SerializeField, Range(1, 5)] public int EventsCount { get; set; } = 3;
    [field: SerializeField, Range(1, 5)] public int MinSuccessfulEventCount { get; set; } = 1;
    [field: SerializeField, Range(1, 2)] public float EventDuration { get; private set; } = 1.5f;

    private void OnValidate() {
        MinSuccessfulEventCount = Mathf.Clamp(MinSuccessfulEventCount, 0, EventsCount);
    }
}
