using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //무기ID, 프리펩ID, 데미지, 개수, 속도 변수
    public int ID;
    public int prefabID;
    public float damage;
    public int count;
    public float speed;

    //타이머
    float timer;

    //플레이어 변수
    Player player;

    private void Awake()
    {
        //GetComponentInParent : 함수로 부모의 컴포넌트 가져오기
        //player = GetComponentInParent<Player>();
        player = GameManager.instance.player;
    }

    private void Update()
    {
        switch (ID)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                timer += Time.deltaTime;

                //speed보다 커지면 초기화하면서 발사로직 실행
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
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

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

    }

    //Weapon 초기화 함수에 스크립터블 오브젝트를 매개변수로 받아 활용
    public void Init(ItemData data)
    {
        //Basic Set
        name = "Weapon " + data.itemID;
        transform.parent = player.transform;

        //지역위치인 localPosition을 원점으로 변경
        transform.localPosition = Vector3.zero;

        //Property Set
        //초기화
        ID = data.itemID;
        damage = data.baseDamage;
        count = data.baseCount;
        
        for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabID =i;
                break;
            }
        }

        switch (ID)
        {
            case 0:
                speed = 150;
                Batch();
                break;

            default:
                //speed값은 연사속도를 의미: 적을수록 많이 발사
                speed = 0.4f;
                break;
        }

        //Hand Set
        //enum 값 앞에 int타입을 작성하여 강제 형변환
        Hand hand = player.hands[(int)data.itemType];

        hand.spriter.sprite = data.hand;

        hand.gameObject.SetActive(true);

        //BroadcastMessage : 특정 함수 호출을 모든 자식에게 방송하는 함수
        //BroadcastMessage 의 두번째 인자값으로 DontRequireReceiver 추가
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    //생성된 무기를 배치하는 함수생성 및 호출
    private void Batch()
    {
        //for문으로 count만큼 풀링에서 가져오기
        for(int i = 0; i < count; i++)
        {
            //기본 오브젝트를 먼저 활용하고 모자란 것은 풀링에서 가져오기.
            Transform bullet; 
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabID).transform;

                //paerent 속성을 통해 부모 변경
                bullet.parent = transform;
            }

            //초기화함수
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;

            //Rotate : 함수로 계산된 각도
            bullet.Rotate(rotVec);

            //Translate : 함수로 자신의 위쪽으로 이독
            //Space.World 방향 
            bullet.Translate(bullet.up * 1.5f,Space.World);

            //속성 초기화함수
            bullet.GetComponent<Bullet>().Init(damage, -1,Vector3.zero); // -1 is Infinity per.
        }
    }

    private void Fire()
    {
        //저정된 목표가 없으면 넘어가는 로직
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;

        //크기가 포함된 방향 : 목표위치 - 나의 위치
        Vector3 dir = targetPos - transform.position;

        //normalized : 현재 벡터의 방향은 유지하고 크기를 1로 변한된 속성
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabID).transform;

        //기존 생성 로직을 그대로 활용하면서 위치는 플레이어 위치로 지정
        bullet.position = transform.position;

        //FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        //원거리 공격에 맞게 초기화 함수 호출하기
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }

}
