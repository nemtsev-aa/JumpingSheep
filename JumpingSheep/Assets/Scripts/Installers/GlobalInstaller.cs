using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller {
    [SerializeField] private GameplayConfig _gameplayConfig;
    [SerializeField] private QTEEventConfigs _qTEEventConfigs;
    [SerializeField] private SheepFactory _sheepFactory;

    [SerializeField] private UICompanentPrefabs _uiCompanentPrefabs;
    [SerializeField] private LevelConfigs _levelConfig;
    //[SerializeField] private DifficultyLevelsConfig _difficultyLevelsConfig;
    //[SerializeField] private QuestionsConfig _questionsConfig;
    //[SerializeField] private DrawingsConfig _drawingsConfig;
    //[SerializeField] private TrainingGameConfigs _trainingGameConfigs;
    //[SerializeField] private Transform _linesParent;
    //[SerializeField] private Pointer _pointerPrefab;

    public override void InstallBindings() {
        BindConfigs();
        BindUICompanentsConfig();

        //BindPointer();

        BindFactories();
        //BindTimeCounter();
        BindInput();
        BindLogger();
    }

    private void BindConfigs() {
        if (_levelConfig.Configs.Count == 0)
            Debug.LogError($"List of LevelConfig is empty");

        Container.Bind<LevelConfigs>().FromInstance(_levelConfig).AsSingle();

        if (_qTEEventConfigs.Configs.Count == 0)
            Debug.LogError($"List of QTEEventConfigs is empty");

        Container.Bind<QTEEventConfigs>().FromInstance(_qTEEventConfigs).AsSingle();
    }

    private void BindUICompanentsConfig() {
        if (_uiCompanentPrefabs.Prefabs.Count == 0)
            Debug.LogError($"List of UICompanentPrefabs is empty");

        Container.Bind<UICompanentPrefabs>().FromInstance(_uiCompanentPrefabs).AsSingle();
    }

    private void BindFactories() {
        Container.Bind<SheepFactory>().FromInstance(_sheepFactory).AsSingle();
        Container.Bind<DialogFactory>().AsSingle();
        Container.Bind<UICompanentsFactory>().AsSingle();
    }

    //private void BindTimeCounter() {
    //    TimeCounter timeCounter = new TimeCounter();

    //    Container.BindInstance(timeCounter).AsSingle();
    //    Container.BindInterfacesAndSelfTo<ITickable>().FromInstance(timeCounter).AsSingle();
    //}

    //private void BindPointer() {
    //    Pointer pointer = Container.InstantiatePrefabForComponent<Pointer>(_pointerPrefab);
    //    Container.Bind<Pointer>().FromInstance(pointer).AsSingle();
    //}


    private void BindInput() {
        SwipeHandler swipeHandler;

        if (SystemInfo.deviceType == DeviceType.Handheld) {
            MobileInput input = new MobileInput();
            swipeHandler = new SwipeHandler(input);
        }
        else 
        {
            DesktopInput input = new DesktopInput();
            swipeHandler = new SwipeHandler(input);
        }
            
        Container.Bind<SwipeHandler>().FromInstance(swipeHandler).AsSingle().NonLazy(); 
    }

    private void BindLogger() {
        Logger logger = new Logger();
        Container.Bind<Logger>().FromInstance(logger).AsSingle().NonLazy();
    }
}