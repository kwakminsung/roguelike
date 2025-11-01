using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>()
            .Where(t => t != transform)
            .ToArray();
    }
    void Update()
    {
        if (spawnData == null || spawnData.Length == 0) {
            Debug.Log("Spawner: spawnData 배열이 비어 있음");
            return;
        }
        if (spawnPoint == null || spawnPoint.Length == 0) return;
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(Gamemanager.instance.GameTime / 10f), spawnData.Length -1);
        
        if (timer > spawnData[level].spawnTime) {
            Debug.Log($"Spawner: Spawn 실행! Level = {level}, GameTime = {Gamemanager.instance.GameTime}");
            timer = 0;
            Spawn();
        }
    }
    void Spawn()
    {
        GameObject enemy = Gamemanager.instance.enemyPool.Get(level);
        int index = Random.Range(0, spawnPoint.Length);
        enemy.transform.position = spawnPoint[index].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}
