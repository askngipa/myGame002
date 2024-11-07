using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //함수가 아닌 속성을 작성
    public static float Speed
    {
        //삼항연산자 사용
        get { return GameManager.instance.playerID == 0 ? 1.1f : 1f; }
    }
    
    public static float WeaponSpeed
    {
        get { return GameManager.instance.playerID == 1 ? 1.1f : 1f; }
    }
    
    public static float WeaponRate
    {
        get { return GameManager.instance.playerID == 1 ? 0.9f : 1f; }
    }
    
    public static float Damage
    {
        get { return GameManager.instance.playerID == 0 ? 1.5f : 1f; }
    }
    
    public static int Count
    {
        get { return GameManager.instance.playerID == 0 ? 1 : 0; }
    }
}
