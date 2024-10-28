using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Instantiate : ����
// Destroy : �ı�
// Instantiate + Destroy ���־��̸� �޸𸮿� ������ ����

public class PoolManager : MonoBehaviour
{
    //������ ������ ����
    public GameObject[] prefabs;

    // Ǯ��� ����Ʈ
    List<GameObject>[] pools;

    private void Awake()
    {
        //����Ʈ �迭 ���� �ʱ�ȭ�� �� ũ��� ������ �迭���� Ȱ��
        pools = new List<GameObject>[prefabs.Length];

        //�ݺ����� ���� ��� ������Ʈ Ǯ ����Ʈ�� �ʱ�ȭ
        for(int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    //���� ������Ʈ�� ��ȯ�ϴ� �Լ� ����
    //������ ������Ʈ ������ �����ϴ� �Ű����� �߰�
    public GameObject Get(int i)
    {
        GameObject select = null;

        //������ Ǯ�� ���(��Ȱ��ȭ ��) �ִ� ���ӿ�����Ʈ ����

        //�迭, ����Ʈ���� �����͸� ���������� �����ϴ� �ݺ���
        foreach(GameObject item in pools[i])
        {
            //������Ʈ�� ���������� Ȯ��
            if (!item.activeSelf)
            {
                //�߰��ϸ� select ������ �Ҵ�
                select = item;

                //������ ������Ʈ�� ã���� SetActive �Լ��� Ȱ��ȭ
                select.SetActive(true);
                break;
            }
        }


        //��ã������
        if (!select)
        {
            //���Ӱ� ������ select������ �Ҵ�
            select = Instantiate(prefabs[i], transform);

            //������ ������Ʈ�� �ش� ������Ʈ Ǯ ����Ʈ�� Add �Լ��� �߰�
            pools[i].Add(select);
        }

        return select;
    }
}
