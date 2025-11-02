using UnityEditor.Rendering;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabid;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = Gamemanager.instance.player;
    }
    void Update()
    {
        switch (id) {
            case 0:
                transform.Rotate(Vector3.back * speed);
                    break;
            default:
                timer += Time.deltaTime;

                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //if (Input.GetButtonDown("Jump"))
        //    Levelup(20, 10);
    }
    public void Levelup(float damage, int count)
    {
        this.damage = damage;
        this.count = count;

        if(id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        name = "weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int index = 0; index < Gamemanager.instance.enemyPool.prefabs.Length; index++) {
            if (data.projectile == Gamemanager.instance.enemyPool.prefabs[index]) {
                prefabid = index;
                break;
            }
        }

        switch (id) {
            case 0:
                speed = -5;
                Batch();
                break;
            default:
                speed = 0.5f;
                break;
        }

        player.BroadcastMessage("ApplyGear");
    }

    void Batch()
    {
        Poolmanager pool = (id == 0) ? Gamemanager.instance.meleePool : Gamemanager.instance.rangedPool;

            for (int index = 0; index < count; index++)
            {
                Transform bullet;

                if (index < transform.childCount)
                    bullet = transform.GetChild(index);
                else
                    bullet = pool.Get(prefabid).transform;

                bullet.parent = transform;
                bullet.localPosition = Vector3.zero;
                bullet.localRotation = Quaternion.identity;

                Vector3 rotVec = Vector3.forward * 360 * index / count;
                bullet.Rotate(rotVec);
                bullet.Translate(bullet.up * 1.5f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }

        //if (Gamemanager.instance == null || Gamemanager.instance.weaponPool == null)
        //{
        //    Debug.LogWarning("Weapon.Batch: weaponPool not assigned in Gamemanager");
        //}
        //for (int index=0; index < count; index++){
        //    Transform bullet;

        //    if (index < transform.childCount)
        //    {
        //        bullet = transform.GetChild(index);
        //    }
        //    else
        //    {
        //        bullet = Gamemanager.instance.weaponPool.Get(prefabid).transform;
        //        bullet.parent = transform;
        //    }

        //    bullet.localPosition = Vector3.zero;
        //    bullet.localRotation = Quaternion.identity;

        //    Vector3 rotVec = Vector3.forward * 360 * index / count;
        //    bullet.Rotate(rotVec);
        //    bullet.Translate(bullet.up * 1.5f, Space.World);
        //    bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); //-1 is infinity per

        //}
    }

    void Fire() {
        if (!player.scanner.nearestTarget)
                return;

            Poolmanager pool = (id == 0) ? Gamemanager.instance.meleePool
                                         : Gamemanager.instance.rangedPool;

            if (prefabid < 0 || prefabid >= pool.prefabs.Length)
                return;

            Vector3 targetPos = player.scanner.nearestTarget.position;
            Vector3 dir = (targetPos - transform.position).normalized;

            Transform bullet = pool.Get(prefabid).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(damage, count, dir);

        //if (!player.scanner.nearestTarget)
        //    return;

        //if (Gamemanager.instance == null || Gamemanager.instance.weaponPool == null)
        //    return;

        //if (prefabid < 0 || prefabid >= Gamemanager.instance.weaponPool.prefabs.Length)
        //{
        //    Debug.LogError($"Weapon.Fire: Àß¸øµÈ prefabid {prefabid}");
        //}

        //Vector3 targetPos = player.scanner.nearestTarget.position;
        //Vector3 dir = (targetPos - transform.position).normalized;

        //Transform bullet = Gamemanager.instance.weaponPool.Get(prefabid).transform;
        //bullet.position = transform.position;
        //bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        //bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
