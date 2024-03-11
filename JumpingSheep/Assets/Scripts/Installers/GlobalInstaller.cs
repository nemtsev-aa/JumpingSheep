using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller {
    [SerializeField] private UICompanentPrefabs _uiCompanentPrefabs;
    //[SerializeField] private ModsConfig _modsConfig;
    //[SerializeField] private DifficultyLevelsConfig _difficultyLevelsConfig;
    //[SerializeField] private QuestionsConfig _questionsConfig;
    //[SerializeField] private DrawingsConfig _drawingsConfig;
    //[SerializeField] private TrainingGameConfigs _trainingGameConfigs;
    //[SerializeField] private Transform _linesParent;
    //[SerializeField] private Pointer _pointerPrefab;

    public override void InstallBindings() {
        //BuildModsConfig();
        BindUICompanentsConfig();

        //BindPointer();

        BindFactories();
        //BindTimeCounter();
        BindInput();
    }

    //private void BuildModsConfig() {
    //    if (_modsConfig.Configs.Count == 0)
    //        Debug.LogError($"List of ModsConfig is empty");

    //    Container.Bind<ModsConfig>().FromInstance(_modsConfig).AsSingle();
    //}

    private void BindUICompanentsConfig() {
        if (_uiCompanentPrefabs.Prefabs.Count == 0)
            Debug.LogError($"List of UICompanentPrefabs is empty");

        Container.Bind<UICompanentPrefabs>().FromInstance(_uiCompanentPrefabs).AsSingle();
    }

    private void BindFactories() {
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
}