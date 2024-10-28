using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    //범위, 레이어, 스캔결과배열, 가장가까운 목표를 담을변수
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        //CircleCastAll : 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        //1.캐스팅 시작위치
        //2.원의 반지름
        //3.캐스팅 방향
        //4.캐스팅 길이
        //5.대상 레이어
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        //지속적으로 업데이트
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        //foreach문으로 캐스팅 결과 오브젝트를 하나씩 접근
        foreach (RaycastHit2D target in targets)
        {
            Vector3 mypos = transform.position;
            Vector3 targetPos = target.transform.position;

            //Distance(A, B) : 벡터 A 와 B의 거리를 계산해주는 함수
            float curDiff = Vector3.Distance(mypos, targetPos);

            //반복문 돌다가 가져온 거리가 저장된 거리보다 작으면 교체
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }

}
