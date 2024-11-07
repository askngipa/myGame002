using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� ������ ���
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Header : �ν������� �Ӽ����� �̻ڰ� ���н����ִ� Ÿ��Ʋ
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Game Info")]
    public int playerID;
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
    public Result uiResult;
    public GameObject enemyCleaner;

    private void Awake()
    {
        instance = this;
    }

    public void GameStart(int id)
    {
        playerID = id;

        health = maxHealth;

        //���� �����Ҷ� �÷��̾� Ȱ��ȭ �� �⺻ ���� ����
        player.gameObject.SetActive(true);

        //���� ���� ������ ���� �Լ�ȣ�⿡�� ���� ���� ĳ����ID�� ����
        uiLevelUp.Select(playerID % 2);
        Resume();
    }


    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    //�����̸� ���� ���ӿ��� �ڷ�ƾ�� �ۼ�
    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        //���Ӱ�� UI ������Ʈ�� ���ӿ��� �ڷ�ƾ���� Ȱ��ȭ
        uiResult.gameObject.SetActive(true);

        uiResult.Lose();
        Stop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    //�����̸� ���� ���ӿ��� �ڷ�ƾ�� �ۼ�
    IEnumerator GameVictoryRoutine()
    {
        isLive = false;

        //���� �¸� �ڷ�ƾ�� ���ݺο� �� Ŭ���ʸ� Ȱ��ȭ
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        //���Ӱ�� UI ������Ʈ�� ���ӿ��� �ڷ�ƾ���� Ȱ��ȭ
        uiResult.gameObject.SetActive(true);

        uiResult.Win();
        Stop();
    }

    public void GameRetry()
    {
        //LoadScene : �̸� Ȥ�� �ε����� ����� ���Ӱ� �θ��� �Լ�
        SceneManager.LoadScene(0);
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

            //���ӽð��� �ִ�ð��� �ѱ�� ���� ���ӽ¸� �Լ� ȣ��
            GameVictory();
        }
    }

    //����ġ ���� �Լ�
    public void GetExp()
    {
        //����ġ�� ��� �Լ����� isLive ���� �߰�
        if (!isLive)
            return;

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
