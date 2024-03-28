using UnityEngine;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller {
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private SheepPrefabs _sheepPrefabs;
    
    public override void InstallBindings() {
        BindServices();
        BindFactories();
        BindSheepSpawner();
    }

    private void BindServices() => Container.Bind<TimeCounter>().AsSingle();
    
    private void BindFactories() {
        Container.Bind<SheepPrefabs>().FromInstance(_sheepPrefabs).AsSingle();
        Container.Bind<SheepFactory>().AsSingle();
    }

    private void BindSheepSpawner() {
        Container.Bind<Transform>().FromInstance(_spawnPoint).AsSingle();
        Container.Bind<SheepSpawner>().AsSingle();
    }
}
