using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressLoader {
    private const string Key = "LevelProgressData";

    private List<LevelProgressData> _levelProgress;
    private SavesManager _savesManager;

    public ProgressLoader(SavesManager savesManager) {
        _savesManager = savesManager;

        LoadLevelProgressFromJson();
    }

    public IReadOnlyList<LevelProgressData> LevelProgress => _levelProgress;

    public void SaveLevelProgress(List<LevelProgressData> progress) {
        _savesManager.Save(Key, progress, OnLevelProgressSaved);
    }

    private void OnLevelProgressSaved(bool status) {
        if (status == false) {
            Debug.Log("Save falled");
            return;
        }

        Debug.Log("Save complited");
    }

    private void LoadLevelProgressFromJson() {
        _savesManager.Load<List<LevelProgressData>>(Key, OnLevelProgressLoaded);
    }

    private void OnLevelProgressLoaded(List<LevelProgressData> levelProgress) {
        if (levelProgress == null) {
            Debug.LogError($"{Key}.json not found or empty");
            return;
        }

        if (levelProgress.Count() == 0) {
            Debug.LogWarning($"List {Key} in Json-file is empty");
            return;
        }

        _levelProgress = levelProgress.ToList();
        Debug.Log("Load complited");
    }

}
