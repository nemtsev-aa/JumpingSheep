using UnityEngine;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller {
    [SerializeField] private Transform _spawnPoint;

    public override void InstallBindings() {
        BindServices();
        BindFactories();
        BindSheepSpawner();
    }

    private void BindServices() {
        Container.Bind<TimeCounter>().AsSingle();
    }

    private void BindFactories() {
        Container.Bind<SheepFactory>().AsSingle();
    }

    private void BindSheepSpawner() {
        Transform spawnPoint = Container.InstantiatePrefabForComponent<Transform>(_spawnPoint);

        Container.Bind<Transform>().FromInstance(spawnPoint).AsSingle();
        Container.Bind<SheepSpawner>().AsSingle();
    }
}
