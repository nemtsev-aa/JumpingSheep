using UnityEngine;

[CreateAssetMenu(fileName = nameof(SaveManagerConfig), menuName = "Configs/" + nameof(SaveManagerConfig))]
public class SaveManagerConfig : ScriptableObject {
    [field: SerializeField] public SaveType SaveType { get; private set; }
    public string SavePath { get; private set; }

    public void SetPath(string path) {
        if (path != "")
            SavePath = path;
    }
}
