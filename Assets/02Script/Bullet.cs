using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rig;

    private void Awake()
    {
        //�ʱ�ȭ�Լ�
        rig = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
                                            //�Լ��ʱ�ȭ �ӵ� �Ű����� 
    {
        //this �ش� Ŭ������ ������ ����
        this.damage = damage;
        this.per = per;

        //���� -1(����)���� ū�Ϳ� ���ؼ��� �ӵ� ����
        if (per > -1)
        {
            //�ӷ��� �����־� �Ѿ��� ���ư��� �ӵ� ������Ű��
            rig.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || per == -1)
            return;

        per--;

        //���� ���� �ϳ��� �پ��鼭 -1�� �Ǹ� ��Ȱ��ȭ
        if(per == -1)
        {
            //��Ȱ��ȭ�� �����ӵ� �ʱ�ȭ
            rig.velocity = Vector2.zero;

            gameObject.SetActive(false);
        }
    }
}
