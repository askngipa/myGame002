using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//장면 관리를 사용
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Header : 인스펙터의 속성들을 이쁘게 구분시켜주는 타이틀
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

        //게임 시작할때 플레이어 활성화 후 기본 무기 지급
        player.gameObject.SetActive(true);

        //기존 무기 지급을 위한 함수호출에서 인자 값을 캐릭터ID로 변경
        uiLevelUp.Select(playerID % 2);
        Resume();
    }


    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    //딜레이를 위해 게임오버 코루틴도 작성
    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        //게임결과 UI 오브젝트를 게임오버 코루틴에서 활성화
        uiResult.gameObject.SetActive(true);

        uiResult.Lose();
        Stop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    //딜레이를 위해 게임오버 코루틴도 작성
    IEnumerator GameVictoryRoutine()
    {
        isLive = false;

        //게임 승리 코루틴의 전반부에 적 클리너를 활성화
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        //게임결과 UI 오브젝트를 게임오버 코루틴에서 활성화
        uiResult.gameObject.SetActive(true);

        uiResult.Win();
        Stop();
    }

    public void GameRetry()
    {
        //LoadScene : 이름 혹은 인덱스로 장면을 새롭게 부르는 함수
        SceneManager.LoadScene(0);
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

            //게임시간이 최대시간을 넘기는 때에 게임승리 함수 호출
            GameVictory();
        }
    }

    //경험치 증가 함수
    public void GetExp()
    {
        //경험치를 얻는 함수에도 isLive 필터 추가
        if (!isLive)
            return;

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
