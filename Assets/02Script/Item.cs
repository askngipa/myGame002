using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        //자식 오브젝트의 컴포넌트가 필요하므로 GetComponentsInChildren 사용
        //GetComponentsInChildren에서 두번째 값으로 가져오기 (첫번째는 자기자신)
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];

        textName.text = data.itemName;
    }

    private void LateUpdate()
    {
        textLevel.text = "Lv." + (level + 1);
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:

                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();

                    //AddComponent<T> : 게임오브젝트에 T컴포넌트를 추가하는 함수
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }

                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    //처음 이후의 레벨업은 데미지 횟수를 계산
                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:

                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;

            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;

        }

        if (level == data.damages.Length)
        {
            //스크립터블 오브젝트에 작성한 레벨 데이터 개수를 넘기지 않게 로직 추가
            GetComponent<Button>().interactable = false;
        }
    }
}
