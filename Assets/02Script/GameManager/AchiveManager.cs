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
        //Enum.GetValues : �־��� �������� �����͸� ��� �������� �Լ�
        //Enum.GetValues �տ� Ÿ���� ��������� �����Ͽ� Ÿ�� ���߱�
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        //HasKey�Լ��� ������ ����üũ �� �ʱ�ȭ ����
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    private void Init()
    {
        //PlayerPrefs : ������ ���� ����� �����ϴ� ����Ƽ���� Ŭ����
        PlayerPrefs.SetInt("MyData", 1);

        //foreach�� Ȱ���Ͽ� ���������� ������ ����
        foreach(Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
            
        }

        //���������Ϳ� ������ �̸��� key�� 0�� ����
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

            //GetInt �Լ��� ����� ���� ���¸� �����ͼ� ��ư Ȱ��ȭ�� ����
            lockCharacter[i].SetActive(!isUnlock);
            unLockCharacter[i].SetActive(isUnlock);
        }
    }

    //��� ������ Ȯ���ϱ� ���� �ݺ���
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

        //switch~case������ �� ���� �޼� ������ �ۼ�
        switch (achive)
        {
            case Achive.UnLockPotato:
                isAchive = GameManager.instance.kill >= 10; 
                break;

            case Achive.UnLockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }

        //�ش� ������ ó�� �޼� �ߴٴ� ����
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);   
        }
    }

    //�˸�â�� Ȱ��ȭ�ߴٰ� ���� �ð� ���� ��Ȱ��ȭ�ϴ� �ڷ�ƾ ����
    IEnumerator NoticeRoutine()
    {

        yield return wait;

    }

}
