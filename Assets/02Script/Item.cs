using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;

    Image icon;
    Text textLevel;

    private void Awake()
    {
        //�ڽ� ������Ʈ�� ������Ʈ�� �ʿ��ϹǷ� GetComponentsInChildren ���
        //GetComponentsInChildren���� �ι�° ������ �������� (ù��°�� �ڱ��ڽ�)
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
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

                    //AddComponent<T> : ���ӿ�����Ʈ�� T������Ʈ�� �߰��ϴ� �Լ�
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }

                break;

            case ItemData.ItemType.Glove:
                break;

            case ItemData.ItemType.Shoe:
                break;

            case ItemData.ItemType.Heal:
                break;

        }

        level++;

        if (level == data.damages.Length)
        {
            //��ũ���ͺ� ������Ʈ�� �ۼ��� ���� ������ ������ �ѱ��� �ʰ� ���� �߰�
            GetComponent<Button>().interactable = false;
        }
    }
}
