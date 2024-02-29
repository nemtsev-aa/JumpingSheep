using UnityEngine;

[CreateAssetMenu(fileName =nameof(SheepFactory), menuName = "Factories/" + nameof(SheepFactory))]
public class SheepFactory : ScriptableObject {
    [SerializeField] private Sheep _sheepPrefab;

    public Sheep Get(Transform spawnPoint) {
        Sheep newSheep = Instantiate(_sheepPrefab, spawnPoint.position, spawnPoint.rotation);
        return newSheep;
    }
}
