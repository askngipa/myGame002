using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        //Basic set
        name = "Gear" + data.itemID;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        //Property set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;

        //��� ���Ӱ� �߰��ǰų� ������ �� �� �������� �Լ��� ȣ��
        ApplyGear();
    }

    private void ApplyGear()
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

    private void RateUp()
    {
        //�÷��̾�� �ö󰡼� ��� Weapon�� ��������
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        //foreach������ �ϳ��� ��ȸ�ϸ鼭 Ÿ�Կ� ���� �ӵ� �ø���
        foreach(Weapon weapon in weapons)
        {
            switch (weapon.ID)
            {
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = 150 + (150 * rate);
                    break;

                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    //�Ź� ����� ���ǵ���� ����
    private void SpeedUp()
    {
        float speed = 3 * Character.Speed;  
        GameManager.instance.player.Speed = speed + speed * rate;
    }
}
