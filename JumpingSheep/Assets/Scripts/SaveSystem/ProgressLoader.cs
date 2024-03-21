using System.Collections.Generic;
using System.Linq;

public class ProgressLoader {
    private const string Key = "LevelProgressData";

    private List<LevelProgressData> _levelProgress;
    private SavesManager _savesManager;
    private Logger _logger;

    public ProgressLoader(SavesManager savesManager, Logger logger) {
        _savesManager = savesManager;
        _logger = logger;

        LoadLevelProgressFromJson();
    }

    public IReadOnlyList<LevelProgressData> LevelProgress => _levelProgress;

    public void SaveLevelProgress(List<LevelProgressData> progress) {
        _savesManager.Save(Key, progress, OnLevelProgressSaved);
    }

    private void OnLevelProgressSaved(bool status) {
        if (status == false) 
            return;

        _logger.Log("Save complited");
    }

    private void LoadLevelProgressFromJson() {
        _savesManager.Load<List<LevelProgressData>>(Key, OnLevelProgressLoaded);

        if (_levelProgress == null || _levelProgress.Count() == 0)
            _logger.Log($"{Key}.json not found or empty");
    }

    private void OnLevelProgressLoaded(List<LevelProgressData> levelProgress) {
        _levelProgress = levelProgress.ToList();

        _logger.Log("Load complited");

    }
}
