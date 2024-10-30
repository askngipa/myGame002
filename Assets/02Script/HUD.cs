using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType {Exp,Level,Kill,Time,Health }
    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            //현재 경험치 / 최대 경험치
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = curExp / maxExp;
                break;

            case InfoType.Level:
                //Format : 각 숫자 인자값을 지정된 형태의 문자열로 만들어주는 함수
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level); //Lv.{0:F0} 소수점자리 지정
                break;

            case InfoType.Kill:
                myText.text = string.Format("Kill.{0:F0}", GameManager.instance.kill);
                break;

            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;

                //60으로 나누어 분을 구하되 FloorToInt로 소수점 버리기
                int min = Mathf.FloorToInt(remainTime / 60);

                //A % B:A를B로 나누고 남은 나머지
                int sec= Mathf.FloorToInt(remainTime % 60);
                break;

            case InfoType.Health:

                break;
        }
    }
}
