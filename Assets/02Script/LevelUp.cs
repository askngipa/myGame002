using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();

        rect.localScale = Vector3.one;

        //���� �� â�� ��Ÿ���ų� ������� Ÿ�̹� ����
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int i)
    {
        items[i].OnClick(); 
    }

    //������ ��ũ��Ʈ�� ���� Ȱ��ȭ �Լ� �ۼ�
    private void Next()
    {
        //1. ��� ������ ��Ȱ��ȭ
        //foreach�� Ȱ���Ͽ� ��� ������ ������Ʈ ��Ȱ��ȭ
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        //2. ���߿��� ���� 3�� ������ Ȱ��ȭ
        //�������� Ȱ��ȭ �� �������� �ε��� 3���� ���� �迭����
        int[] ran = new int[3];
        while (true)
        {
            ran[0]= Random.Range(0, items.Length);
            ran[1]= Random.Range(0, items.Length);
            ran[2]= Random.Range(0, items.Length);


            //���� ���Ͽ� ��� ���� ������ �ݺ����� ������������ ����
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        //������ Ȱ��ȭ
        for(int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            //3. ���� �������� ���� �Һ���������� ��ü
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);    
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
    