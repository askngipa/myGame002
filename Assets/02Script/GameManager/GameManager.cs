using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Header : �ν������� �Ӽ����� �̻ڰ� ���н����ִ� Ÿ��Ʋ
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Game Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;

    private void Awake()
    {
        instance = this;
    }

    public void GameStart()
    {
        health = maxHealth;

        //�ӽ� ��ũ��Ʈ(ù��° ĳ���� ����)
        uiLevelUp.Select(0);
        isLive = true;
    }

    private void Update()
    {
        //�� ��ũ��Ʈ�� Update�迭 ������ ���� �߰��ϱ�
        if (!isLive)
            return;

        //�캯�� ��� ���Ѵ�
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;

        }
    }

    //����ġ ���� �Լ�
    public void GetExp()
    {
        exp++;
        //Min �Լ��� ����Ͽ� �ְ� ����ġ�� �״�� ����ϵ��� ����
        if (exp == nextExp[Mathf.Min(level,nextExp.Length-1)])
        {
            level++;
            exp = 0;

            //���ӸŴ����� ���� �� ������ â�� �����ִ� �Լ�ȣ��
            uiLevelUp.Show();

        }
    }
       
    //�ð�����
    public void Stop()
    {
        isLive = false;

        //timeScale : ����Ƽ�� �ð� �ӵ� (����)
        Time.timeScale = 0;
    }

    //�۵�
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

}
