using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rig;
    Animator anim;
    SpriteRenderer spriter;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(!isLive)
            return;

        //위치차이 = 타겟위치 - 나의위치
        Vector2 dirVec = target.position - rig.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime;

        //플레이어의 키입력 값을 더한 이동 = 몬스터의 방향값을 더한 이동
        rig.MovePosition(rig.position + nextVec);

        //물리속도가 이동에 영향을 주지 않도록 속도 제거.
        rig.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        //목표의 x축과 자신의 x축의 값을 비교하여 작으면 true가 되도록 설정
        spriter.flipX = target.position.x < rig.position.x;
    }

    //OnEnable에서 타겟 변수에 게임매니저를 활용하여 플레이어에 할당
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true; //생존여부와 체력 초기화
        health = maxHealth; //초기화
    }

    //초기 속성을 적용하는 함수 추가
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    //OnTriggerEnter2D 매개변수의 태그조건으로 활용
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        //컴포넌트"bullet"으로 접근하여 데미지를 가져와 피격 계산
        health -= collision.GetComponent<Bullet>().damage;

        //남은체력을 조건으로 피격과 사망으로 로직으로 나누기
        if (health > 0)
        {
            //Live, Hit, Action
        }
        else
        {
            //die..
            Dead();
        }

    }

    private void Dead()
    {
        //사망할땐 SetActive 함수를 통한 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
