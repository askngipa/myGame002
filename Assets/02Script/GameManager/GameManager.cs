using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Header : 인스펙터의 속성들을 이쁘게 구분시켜주는 타이틀
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

        //임시 스크립트(첫번째 캐릭터 선택)
        uiLevelUp.Select(0);
        isLive = true;
    }

    private void Update()
    {
        //각 스크립트의 Update계열 로직에 조건 추가하기
        if (!isLive)
            return;

        //우변을 계속 더한다
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;

        }
    }

    //경험치 증가 함수
    public void GetExp()
    {
        exp++;
        //Min 함수를 사용하여 최고 경험치를 그대로 사용하도록 변경
        if (exp == nextExp[Mathf.Min(level,nextExp.Length-1)])
        {
            level++;
            exp = 0;

            //게임매니저의 레벨 업 로직에 창을 보여주는 함수호출
            uiLevelUp.Show();

        }
    }
       
    //시간정지
    public void Stop()
    {
        isLive = false;

        //timeScale : 유니티의 시간 속도 (배율)
        Time.timeScale = 0;
    }

    //작동
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

}
