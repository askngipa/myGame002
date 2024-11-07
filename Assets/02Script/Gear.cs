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

        //장비가 새롭게 추가되거나 레벨업 할 때 로직적용 함수를 호출
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
        //플레이어로 올라가서 모든 Weapon을 가져오기
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        //foreach문으로 하나씩 순회하면서 타입에 따라 속도 올리기
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

    //신발 기능인 스피드업을 구현
    private void SpeedUp()
    {
        float speed = 3 * Character.Speed;  
        GameManager.instance.player.Speed = speed + speed * rate;
    }
}
