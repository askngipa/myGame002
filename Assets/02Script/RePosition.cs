using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePosition : MonoBehaviour
{

    Collision2D coll;

    private void Awake()
    {
        coll = GetComponent<Collision2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag) 
        {
            case "Ground":
                float diffx = playerPos.x - myPos.x;
                float diffy = playerPos.y - myPos.y;
                float dirx = diffx < 0 ? -1 : 1;
                float diry = diffy < 0 ? -1 : 1;
                diffx=Mathf.Abs(diffx);
                diffy=Mathf.Abs(diffy);

                if (diffx > diffy)
                {
                    transform.Translate(Vector3.right * dirx * 40);
                }
                else if (diffx < diffy)
                {
                    transform.Translate(Vector3.up * diry * 40);
                }
                break;

            case "Enemy":

                if (coll.enabled)
                {
                    //플레이어의 이동방향에 따라 맞은 편에서 등장하도록 이동
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3));
                    transform.Translate(ran + dist * 2);
                }
                break;
        }

    }

}
