using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();

        rect.localScale = Vector3.one;

        //레벨 업 창이 나타나거나 사라지는 타이밍 제어
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int i)
    {
        items[i].OnClick(); 
    }

    //아이템 스크립트에 랜덤 활성화 함수 작성
    private void Next()
    {
        //1. 모든 아이템 비활성화
        //foreach를 활용하여 모든 아이템 오브젝트 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        //2. 그중에서 랜덤 3개 아이템 활성화
        //랜덤으로 활성화 할 아이템의 인덱스 3개를 담을 배열선언
        int[] ran = new int[3];
        while (true)
        {
            ran[0]= Random.Range(0, items.Length);
            ran[1]= Random.Range(0, items.Length);
            ran[2]= Random.Range(0, items.Length);


            //서로 비교하여 모두 같지 않으면 반복문을 빠져나가도록 설정
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        //아이템 활성화
        for(int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            //3. 만렙 아이템의 경우는 소비아이템으로 대체
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);    
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
    