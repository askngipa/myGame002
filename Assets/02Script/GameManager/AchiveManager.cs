using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unLockCharacter;
    public GameObject[] uiNotice;

    enum Achive { UnLockPotato,UnLockBean}
    Achive[] achives;
    WaitForSecondsRealtime wait;

    private void Awake()
    {
        //Enum.GetValues : 주어진 열거형의 데이터를 모두 가져오는 함수
        //Enum.GetValues 앞에 타입을 명시적으로 지정하여 타입 맞추기
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        //HasKey함수로 데이터 유무체크 후 초기화 실행
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    private void Init()
    {
        //PlayerPrefs : 간단한 저장 기능을 제공하는 유니티제공 클래스
        PlayerPrefs.SetInt("MyData", 1);

        //foreach를 활용하여 순차적으로 데이터 저장
        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
            
        }

        //업적데이터와 동일한 이름의 key로 0을 저장
        //PlayerPrefs.SetInt("UnLockPotato", 0);
        //PlayerPrefs.SetInt("UnLockBean", 0);
    }

    private void Start()
    {
        UnLockCharacter();
    }

    private void UnLockCharacter()
    {
        for(int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;

            //GetInt 함수로 저장된 업적 상태를 가져와서 버튼 활성화에 적용
            lockCharacter[i].SetActive(!isUnlock);
            unLockCharacter[i].SetActive(isUnlock);
        }
    }

    //모든 업적을 확인하기 위한 반복문
    private void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    private void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        //switch~case문으로 각 업적 달성 조건을 작성
        switch (achive)
        {
            case Achive.UnLockPotato:
                isAchive = GameManager.instance.kill >= 10; 
                break;

            case Achive.UnLockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        //해당 업적이 처음 달성 했다는 조건
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);   
        }
    }

    //알림창을 활성화했다가 일정 시간 이후 비활성화하는 코루틴 생성
    IEnumerator NoticeRoutine()
    {

        yield return wait;

    }

}
