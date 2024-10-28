using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//하나의 스크립트 내에 여러 클래스를 선언가능
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    
    //만든 클래스를 그대로 타입으로 활용하여 배열 변수 선언
    public SpawnData[] spawnData;


    float timer;
    int level;

    private void Awake()
    {
        //GetComponentInChildren:1개 GetComponentsInChildren:여러개
        spawnPoint = GetComponentsInChildren<Transform>();
    }


    private void Update()
    {
        //우변을 계속 더한다
        timer += Time.deltaTime;

        //FloorToInt : 소수점 아래는 버리고 int형으로 바꾸는 함수
        //CeilToInt : 소수점 아래를 올리고 int형으로 바꾸는 함수
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    //따로 함수를 빼서 만드는게 코드에 가독성도 좋고 정리도좋아서
    private void Spawn()
    {
        //Instantiate : 반환 값을 변수에 넣어두기
        //GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, 2));
        GameObject enemy = GameManager.instance.pool.Get(0);

        //GetComponentsInChildren 이거는 자기 자신도 포함이다
        //따라서 자기 자신은 필요가 없기때문에
        //자식 오브젝트에서만 선택되도록 랜덤 시작은 1부터
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;

        //새롭게 작성한 함수를 호출하고 소환데이터 인자값 전달
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

//직렬화(Serialzation) : 개체를 저장 혹은 전송하기 위해 변환
//직접 작성한 클래스를 직렬화를 통해서 인스펙터에서 초기화 가능
[System.Serializable]
public class SpawnData
{
    //추가할 속성들 : 스프라이트 타입, 소환시간, 체력, 속도
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}

    //if (Input.GetButtonDown("Jump"))
    //{
    //    //게임매니저의 인스턴스까지 접근하여 풀링의 함수 호출
    //    GameManager.instance.pool.Get(1);
    //}

    //private void Start()
    //{
    //    GameManager.instance.pool.Get(1);
    //}