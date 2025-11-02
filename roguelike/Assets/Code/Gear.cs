using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data) 
    {
        name = "Gear" + data.itemId;
        transform.parent = Gamemanager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons) {
            if(weapon.id != 0) continue;
            weapon.speed = 150 + (150 * rate);
                //case 0:
                //    weapon.speed = 150 + (150 * rate);
                //    break;
                //default:
                //    weapon.speed = 0.5f + (1f - rate);
                //    break;
        }
    }

    void SpeedUp()
    {
        float speed = 3;
        Gamemanager.instance.player.speed = speed + speed * rate;
    }
}
