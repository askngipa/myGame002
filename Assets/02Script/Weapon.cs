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
            bullet.GetComponent<Bullet>().Init(damage, -1); // -1 is Infinity per.
        }
    }

}
