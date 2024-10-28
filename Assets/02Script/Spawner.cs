using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ϳ��� ��ũ��Ʈ ���� ���� Ŭ������ ���𰡴�
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    
    //���� Ŭ������ �״�� Ÿ������ Ȱ���Ͽ� �迭 ���� ����
    public SpawnData[] spawnData;


    float timer;
    int level;

    private void Awake()
    {
        //GetComponentInChildren:1�� GetComponentsInChildren:������
        spawnPoint = GetComponentsInChildren<Transform>();
    }


    private void Update()
    {
        //�캯�� ��� ���Ѵ�
        timer += Time.deltaTime;

        //FloorToInt : �Ҽ��� �Ʒ��� ������ int������ �ٲٴ� �Լ�
        //CeilToInt : �Ҽ��� �Ʒ��� �ø��� int������ �ٲٴ� �Լ�
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

        if (timer > spawnData[level].spawnTime)
        {
            timer = 0f;
            Spawn();
        }
    }

    //���� �Լ��� ���� ����°� �ڵ忡 �������� ���� ���������Ƽ�
    private void Spawn()
    {
        //Instantiate : ��ȯ ���� ������ �־�α�
        //GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, 2));
        GameObject enemy = GameManager.instance.pool.Get(0);

        //GetComponentsInChildren �̰Ŵ� �ڱ� �ڽŵ� �����̴�
        //���� �ڱ� �ڽ��� �ʿ䰡 ���⶧����
        //�ڽ� ������Ʈ������ ���õǵ��� ���� ������ 1����
        enemy.transform.position = spawnPoint[Random.Range(1,spawnPoint.Length)].position;

        //���Ӱ� �ۼ��� �Լ��� ȣ���ϰ� ��ȯ������ ���ڰ� ����
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

//����ȭ(Serialzation) : ��ü�� ���� Ȥ�� �����ϱ� ���� ��ȯ
//���� �ۼ��� Ŭ������ ����ȭ�� ���ؼ� �ν����Ϳ��� �ʱ�ȭ ����
[System.Serializable]
public class SpawnData
{
    //�߰��� �Ӽ��� : ��������Ʈ Ÿ��, ��ȯ�ð�, ü��, �ӵ�
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}

    //if (Input.GetButtonDown("Jump"))
    //{
    //    //���ӸŴ����� �ν��Ͻ����� �����Ͽ� Ǯ���� �Լ� ȣ��
    //    GameManager.instance.pool.Get(1);
    //}

    //private void Start()
    //{
    //    GameManager.instance.pool.Get(1);
    //}