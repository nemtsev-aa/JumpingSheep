using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller {
    public const float InitDelay = 0.1f;

    [SerializeField] private LevelConfigs _levelConfig;
    [SerializeField] private DialogPrefabs _dialogPrefabs;
    [SerializeField] private UICompanentPrefabs _uiCompanentPrefabs;
    [SerializeField] private SaveManagerConfig _saveConfig;

    [SerializeField] private QTEEventConfigs _qTEEventConfigs;
    [SerializeField] private QTESoundManager _qTESoundManager;

    private Logger _logger;

    public override void InstallBindings() {
        BindServices();
        BindLevelConfigs();
        BindSaveManager();
        BindUIPrefabs();

        BindFactories();
        BindInput();
    }

    private void BindServices() {
        _logger = new Logger();
        Container.Bind<Logger>().FromInstance(_logger).AsSingle();

        Container.Bind<PauseHandler>().AsSingle();
        Container.Bind<Score>().AsSingle();
        Container.Bind<SheepQuantityCounter>().AsSingle();
        Container.Bind<AdManager>().AsSingle();

        if (_qTEEventConfigs.Configs.Count == 0)
            _logger.Log($"List of QTEEventConfigs is empty");

        Container.Bind<QTEEventConfigs>().FromInstance(_qTEEventConfigs).AsSingle();
        Container.Bind<QTESoundManager>().FromInstance(_qTESoundManager).AsSingle();

        Container.BindInterfacesAndSelfTo<QTESystem>().AsSingle();
    }

    private void BindLevelConfigs() {
        if (_levelConfig.Configs.Count == 0)
            _logger.Log($"List of LevelConfig is empty");

        Container.Bind<LevelConfigs>().FromInstance(_levelConfig).AsSingle();
    }

    private void BindSaveManager() {
        if (_saveConfig.SavePath == "")
            _saveConfig.SetPath(Application.persistentDataPath);

        Container.BindInstance(_saveConfig).AsSingle();

        Container.Bind<SavesManager>().AsSingle();
        Container.Bind<ProgressLoader>().AsSingle();
    }

    private void BindUIPrefabs() {
        if (_dialogPrefabs.Prefabs.Count == 0)
            _logger.Log($"List of DialogPrefabs is empty");

        Container.Bind<DialogPrefabs>().FromInstance(_dialogPrefabs).AsSingle();

        if (_uiCompanentPrefabs.Prefabs.Count == 0)
            _logger.Log($"List of UICompanentPrefabs is empty");

        Container.Bind<UICompanentPrefabs>().FromInstance(_uiCompanentPrefabs).AsSingle();
    }

    private void BindFactories() {
        Container.Bind<DialogFactory>().AsSingle();
        Container.Bind<UICompanentsFactory>().AsSingle();
    }

    private void BindInput() {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            Container.BindInterfacesAndSelfTo<MobileInput>().AsSingle();
        else
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();

        Container.Bind<SwipeHandler>().AsSingle().NonLazy();
    }
}