using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //����ID, ������ID, ������, ����, �ӵ� ����
    public int ID;
    public int prefabID;
    public float damage;
    public int count;
    public float speed;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (ID)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 5);
        }
    }

    public void LevelUp(float damage,int count)
    {
        this.damage = damage;
        this.count += count;

        if (ID == 0)
        {
            Batch();
        }
    }

    public void Init()
    {
        switch (ID)
        {
            case 0:
                speed = -150;
                Batch();
                break;

            default:
                break;
        }
    }

    //������ ���⸦ ��ġ�ϴ� �Լ����� �� ȣ��
    private void Batch()
    {
        //for������ count��ŭ Ǯ������ ��������
        for(int i = 0; i < count; i++)
        {
            //�⺻ ������Ʈ�� ���� Ȱ���ϰ� ���ڶ� ���� Ǯ������ ��������.
            Transform bullet; 
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabID).transform;

                //paerent �Ӽ��� ���� �θ� ����
                bullet.parent = transform;
            }

            //�ʱ�ȭ�Լ�
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;

            //Rotate : �Լ��� ���� ����
            bullet.Rotate(rotVec);

            //Translate : �Լ��� �ڽ��� �������� �̵�
            //Space.World ���� 
            bullet.Translate(bullet.up * 1.5f,Space.World);

            //�Ӽ� �ʱ�ȭ�Լ�
            bullet.GetComponent<Bullet>().Init(damage, -1); // -1 is Infinity per.
        }
    }

}
