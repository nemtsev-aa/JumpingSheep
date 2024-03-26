using System;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private GameplayMediator _gameplayMediator;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private EnvironmentSoundManager _environmentSoundManager;
    
    private ProgressLoader _loader;
    private LevelConfigs _configs;

    [Inject]
    public void Construct(ProgressLoader loader, LevelConfigs configs) {
        _loader = loader;
        _configs = configs;
    }

    private void Start() {
        LoadProgress();
            
        _uIManager.Init();
        _environmentSoundManager.Init();
        _gameplayMediator.Init(_uIManager, _environmentSoundManager);
    }

    [ContextMenu(nameof(ResetLocalPlayerProgress))]
    public void ResetLocalPlayerProgress() => _loader.ResetLocalPlayerProgress();
    
    [ContextMenu(nameof(ResetCloudPlayerProgress))]
    public void ResetCloudPlayerProgress() => _loader.ResetCloudPlayerProgress();
    
    private void LoadProgress() {
        _loader.LoadPlayerData();
        var playerData = _loader.PlayerData;

        if (playerData == null) 
            return;

        _configs.UpdateProgress(playerData.LevelProgressDatas);
    }


}
