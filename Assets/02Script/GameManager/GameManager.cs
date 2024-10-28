using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime;
    public float maxGameTime = 2 * 10f;


    public PoolManager pool;
    public Player player;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //�캯�� ��� ���Ѵ�
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;

        }
    }
}