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
        //초기화함수
        rig = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
                                            //함수초기화 속도 매개변수 
    {
        //this 해당 클래스의 변수로 접근
        this.damage = damage;
        this.per = per;

        //관통 -1(무한)보다 큰것에 대해서는 속도 적용
        if (per > -1)
        {
            //속력을 곱해주어 총알이 날아가는 속도 증가시키기
            rig.velocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || per == -1)
            return;

        per--;

        //관통 값이 하나씩 줄어들면서 -1이 되면 비활성화
        if(per == -1)
        {
            //비활성화전 물리속도 초기화
            rig.velocity = Vector2.zero;

            gameObject.SetActive(false);
        }
    }
}
