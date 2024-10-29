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
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    //초기화
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        //GetCurrentAnimatorStateInfo : 현재 상태 정보를 가져오는함수
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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
        coll.enabled = true;
        rig.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
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
        StartCoroutine(KnockBack());

        //남은체력을 조건으로 피격과 사망으로 로직으로 나누기
        if (health > 0)
        {
            //Live, Hit, Action
            //피격 부분에 애니메이션 SetTrigget함수를 호출하여 상태 변경
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;

            //컴포넌트 비활성화는 .enabled = false;
            coll.enabled = false;

            //리지드바디의 물리적 비활성화는 .simulated = false;
            rig.simulated = false;

            //스프라이트 렌더러의 sortingOrder 감소
            //order in layer 감소
            spriter.sortingOrder = 1;

            //SetBool함수를 통해 죽는 애니메이션 상태로 전환
            anim.SetBool("Dead", true);

            Dead();
        }

    }

    //코루틴 Coroutine : 생명주기와 비동기처럼 실행되는 함수
    //IEnumerator : 코루틴만의 반환형 인터페이스
    IEnumerator KnockBack()
    {
        //yield : 코루틴의 반환 키워드
        //yield return 을 통해 다양한 쉬는시간을 지정
        //yield return null; //1프레임 쉬기 //하나의 물리 프레임을 딜레이
        //yield return new WaitForSeconds(2f); //2초 쉬기

        yield return wait;//다음 하나의 물리프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;

        //플레이어 기준의 반대방향 : 현재위치 - 플레이어 위치
        Vector3 dirVec = transform.position - playerPos;

        //리지드바디2D의 AddForce함수로 힘 가하기
        //순간적인 힘이므로 ForceMode2D.Impulse속성 추가.
        rig.AddForce(dirVec.normalized *3,ForceMode2D.Impulse);

    }

    private void Dead()
    {
        //사망할땐 SetActive 함수를 통한 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
