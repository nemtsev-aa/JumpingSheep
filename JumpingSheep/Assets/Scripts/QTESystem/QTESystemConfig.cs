using UnityEngine;

[CreateAssetMenu(fileName = nameof(QTESystemConfig), menuName = "Configs/" + nameof(QTESystemConfig))]
public class QTESystemConfig : ScriptableObject {
    [field: SerializeField, Range(1, 5)] public int EventsCount { get; set; } = 3;
    [field: SerializeField, Range(1, 5)] public int MinSuccessfulEventCount { get; set; } = 1;

    private void OnValidate() {
        MinSuccessfulEventCount = Mathf.Clamp(MinSuccessfulEventCount, 0, EventsCount);
    }
}
