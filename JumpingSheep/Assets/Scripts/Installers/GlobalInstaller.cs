using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller {
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameplayConfig _gameplayConfig;

    [SerializeField] private QTEEventConfigs _qTEEventConfigs;
    [SerializeField] private QTESoundManager _qTESoundManager;

    [SerializeField] private UICompanentPrefabs _uiCompanentPrefabs;
    [SerializeField] private LevelConfigs _levelConfig;
    [SerializeField] private SaveManagerConfig _saveConfig;

    
    //[SerializeField] private DifficultyLevelsConfig _difficultyLevelsConfig;
    //[SerializeField] private QuestionsConfig _questionsConfig;
    //[SerializeField] private DrawingsConfig _drawingsConfig;
    //[SerializeField] private TrainingGameConfigs _trainingGameConfigs;
    //[SerializeField] private Transform _linesParent;
    //[SerializeField] private Pointer _pointerPrefab;

    public override void InstallBindings() {
        BindLogger();
        BindServices();
        BindSaveManager();
        BindLevelConfigs();
        BindUICompanentsConfig();

        //BindPointer();

        BindFactories();
        BindTimeCounter();
        BindSheepSpawner();
        BindInput();
        BindQTESystemCompanents();
        
    }

    private void BindServices() {
        Container.Bind<SheepQuantityCounter>().AsSingle();
        Container.Bind<Score>().AsSingle();
        Container.BindInterfacesAndSelfTo<PauseHandler>().AsSingle();
        Container.Bind<AdManager>().AsSingle();
    }

    private void BindLevelConfigs() {
        if (_levelConfig.Configs.Count == 0)
            Debug.LogError($"List of LevelConfig is empty");

        Container.Bind<LevelConfigs>().FromInstance(_levelConfig).AsSingle();
    }

    private void BindUICompanentsConfig() {
        if (_uiCompanentPrefabs.Prefabs.Count == 0)
            Debug.LogError($"List of UICompanentPrefabs is empty");

        Container.Bind<UICompanentPrefabs>().FromInstance(_uiCompanentPrefabs).AsSingle();
    }

    private void BindFactories() {
        Container.Bind<SheepFactory>().AsSingle();
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

    private void BindLogger() => Container.Bind<Logger>().AsSingle();
    
    private void BindSheepSpawner() {
        Transform spawnPoint = Container.InstantiatePrefabForComponent<Transform>(_spawnPoint);
        
        Container.Bind<Transform>().FromInstance(spawnPoint).AsSingle();
        Container.Bind<SheepSpawner>().AsSingle();
    }

    private void BindQTESystemCompanents() {
        if (_qTEEventConfigs.Configs.Count == 0)
            Debug.LogError($"List of QTEEventConfigs is empty");

        Container.Bind<QTEEventConfigs>().FromInstance(_qTEEventConfigs).AsSingle();
        Container.BindInterfacesAndSelfTo<QTESystem>().AsSingle(); 

        QTESoundManager qTESoundManager = Container.InstantiatePrefabForComponent<QTESoundManager>(_qTESoundManager);
        Container.Bind<QTESoundManager>().FromInstance(qTESoundManager).AsSingle();   
    }

    private void BindTimeCounter() {
        TimeCounter timeCounter = new TimeCounter();

        Container.BindInstance(timeCounter).AsSingle();
        Container.BindInterfacesAndSelfTo<ITickable>().FromInstance(timeCounter).AsSingle();
    }

    private void BindSaveManager() {
        if (_saveConfig.SavePath == "")
            _saveConfig.SetPath(Application.persistentDataPath);

        Container.BindInstance(_saveConfig).AsSingle();

        Container.Bind<SavesManager>().AsSingle();
        Container.Bind<ProgressLoader>().AsSingle();
    }
}