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
            //���� ����ġ / �ִ� ����ġ
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySlider.value = curExp / maxExp;
                break;

            case InfoType.Level:
                //Format : �� ���� ���ڰ��� ������ ������ ���ڿ��� ������ִ� �Լ�
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level); //Lv.{0:F0} �Ҽ����ڸ� ����
                break;

            case InfoType.Kill:
                myText.text = string.Format("Kill.{0:F0}", GameManager.instance.kill);
                break;

            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;

                //60���� ������ ���� ���ϵ� FloorToInt�� �Ҽ��� ������
                int min = Mathf.FloorToInt(remainTime / 60);

                //A % B:A��B�� ������ ���� ������
                int sec= Mathf.FloorToInt(remainTime % 60);
                break;

            case InfoType.Health:

                break;
        }
    }
}
