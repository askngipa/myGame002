using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float Speed;

    //플레이어 스크립트에서 검색 클래스 타입변수 선언및 초기화
    public Scanner scanner;

    public Hand[] hands;

    Rigidbody2D rig;
    SpriteRenderer spriter;
    Animator anim;
    
    private void Awake()
    {
        rig=GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim =GetComponent<Animator>();
        scanner = GetComponent<Scanner>();

        //인자 값 true를 넣으면 비활성화 된 오브젝트도 OK
        hands = GetComponentsInChildren<Hand>(true);
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec * Speed * Time.fixedDeltaTime;
        rig.MovePosition(rig.position + nextVec);
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    //LateUpdate : 프레임이 종료되기 전 실행되는 생명주기 함수
    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        //magnitude : 벡터의 순수한 크기 값
        anim.SetFloat("Speed",inputVec.magnitude);

        if (inputVec.x !=0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    //플레이어 스크립트에 OnCollisionStay2D 이벤트 함수 작성
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        //Time.deltaTime을 활용하여 적절한 피격 데미지 게산
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            //childCount : 자식 오브젝트의 개수
            for (int i = 2; i < transform.childCount; i++)
            {
                //GetChild : 주어진 인덱스의 자식 오브젝트를 반환하는 함수  
                transform.GetChild(i).gameObject.SetActive(false);
            }

            //애니메이터 SetTrigger 함수로 죽음 애니메이션 실행
            anim.SetTrigger("Dead");
        }
    }
}
