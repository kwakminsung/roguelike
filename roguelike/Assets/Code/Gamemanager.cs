using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    [Header("Game Control")]
    public float GameTime;
    public float maxGameTime = 2 * 10f;
    [Header("Player Info")]
    public int health;
    public int maxhealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("Game Object")]
    public Poolmanager enemyPool;
    public Poolmanager meleePool;
    public Poolmanager rangedPool;
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

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
