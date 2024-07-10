using UnityEngine;
using Mirror;

public class CubeGenerator : NetworkBehaviour
{
    public GameObject cubePrefab1; // Префаб первого типа куба
    public GameObject cubePrefab2; // Префаб второго типа куба
    public Vector3 startPosition; // Начальная позиция для генерации
    public float spacing = 1.5f; // Расстояние между кубами

    public override void OnStartServer()
    {
        base.OnStartServer();
        GenerateRandomCubes();
    }

    [Server]
    void GenerateRandomCubes()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Vector3 position = startPosition + new Vector3(x * spacing, 0, y * spacing);
                // Явное указание UnityEngine.Random для разрешения конфликта
                GameObject cubePrefab = (UnityEngine.Random.value > 0.5f) ? cubePrefab1 : cubePrefab2;
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                NetworkServer.Spawn(cube);
            }
        }
    }
}
