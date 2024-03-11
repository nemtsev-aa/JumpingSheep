using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = nameof(UICompanentPrefabs), menuName = "Configs/" + nameof(UICompanentPrefabs))]
public class UICompanentPrefabs : ScriptableObject {
    [field: SerializeField] public List<UICompanent> Prefabs { get; private set; }
}
