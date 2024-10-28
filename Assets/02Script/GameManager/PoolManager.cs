using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Instantiate : 생성
// Destroy : 파괴
// Instantiate + Destroy 자주쓰이면 메모리에 문제가 생김

public class PoolManager : MonoBehaviour
{
    //프리렙 보관할 변수
    public GameObject[] prefabs;

    // 풀담당 리스트
    List<GameObject>[] pools;

    private void Awake()
    {
        //리스트 배열 변수 초기화할 때 크기는 프리펩 배열길이 활용
        pools = new List<GameObject>[prefabs.Length];

        //반복문을 통해 모든 오브젝트 풀 리스트를 초기화
        for(int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    //게임 오브젝트를 반환하는 함수 선언
    //가져올 오브젝트 종류를 결정하는 매개변수 추가
    public GameObject Get(int i)
    {
        GameObject select = null;

        //선택한 풀의 놀고(비활성화 된) 있는 게임오브젝트 접근

        //배열, 리스트들의 데이터를 순차적으로 접근하는 반복문
        foreach(GameObject item in pools[i])
        {
            //오브젝트가 대기상태인지 확인
            if (!item.activeSelf)
            {
                //발견하면 select 변수에 할당
                select = item;

                //대기상태 오브젝트를 찾으면 SetActive 함수로 활성화
                select.SetActive(true);
                break;
            }
        }


        //못찾았으면
        if (!select)
        {
            //새롭게 생성후 select변수에 할당
            select = Instantiate(prefabs[i], transform);

            //생성된 오브젝트는 해당 오브젝트 풀 리스트에 Add 함수로 추가
            pools[i].Add(select);
        }

        return select;
    }
}
