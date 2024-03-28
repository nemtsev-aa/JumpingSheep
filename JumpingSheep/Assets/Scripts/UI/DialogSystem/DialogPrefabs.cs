using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DialogPrefabs), menuName = "Configs/" + nameof(DialogPrefabs))]
public class DialogPrefabs : ScriptableObject {
    [field: SerializeField] public List<Dialog> Prefabs { get; private set; }
}
