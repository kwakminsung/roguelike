using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public float GameTime;
    public float maxGameTime = 2 * 10f;
    public Poolmanager enemyPool;
    public Poolmanager weaponPool;
    public Player player;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        GameTime += Time.deltaTime;
        if (GameTime > maxGameTime)
        {
            GameTime = maxGameTime;
        }
    }
}
