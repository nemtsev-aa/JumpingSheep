using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SheepPrefabs), menuName = "Configs/" + nameof(SheepPrefabs))]
public class SheepPrefabs : ScriptableObject {
    [field: SerializeField] public List<SheepData> DataList { get; private set; }
}
