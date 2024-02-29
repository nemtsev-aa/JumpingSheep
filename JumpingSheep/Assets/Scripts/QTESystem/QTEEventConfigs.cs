using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(QTEEventConfigs), menuName = "Configs/" + nameof(QTEEventConfigs))]
public class QTEEventConfigs : ScriptableObject {
    [SerializeField] private List<QTEEventConfig> _configs = new List<QTEEventConfig>();

    public List<QTEEventConfig> Configs => _configs;
}
