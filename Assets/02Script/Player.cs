using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float Speed;

    //�÷��̾� ��ũ��Ʈ���� �˻� Ŭ���� Ÿ�Ժ��� ����� �ʱ�ȭ
    public Scanner scanner;

    public Hand[] hands;

    Rigidbody2D rig;
    SpriteRenderer spriter;
    Animator anim;
    
    private void Awake()
    {
        rig=GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim =GetComponent<Animator>();
        scanner = GetComponent<Scanner>();

        //���� �� true�� ������ ��Ȱ��ȭ �� ������Ʈ�� OK
        hands = GetComponentsInChildren<Hand>(true);
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        Vector2 nextVec = inputVec * Speed * Time.fixedDeltaTime;
        rig.MovePosition(rig.position + nextVec);
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    //LateUpdate : �������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        //magnitude : ������ ������ ũ�� ��
        anim.SetFloat("Speed",inputVec.magnitude);

        if (inputVec.x !=0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    //�÷��̾� ��ũ��Ʈ�� OnCollisionStay2D �̺�Ʈ �Լ� �ۼ�
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        //Time.deltaTime�� Ȱ���Ͽ� ������ �ǰ� ������ �Ի�
        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            //childCount : �ڽ� ������Ʈ�� ����
            for (int i = 2; i < transform.childCount; i++)
            {
                //GetChild : �־��� �ε����� �ڽ� ������Ʈ�� ��ȯ�ϴ� �Լ�  
                transform.GetChild(i).gameObject.SetActive(false);
            }

            //�ִϸ����� SetTrigger �Լ��� ���� �ִϸ��̼� ����
            anim.SetTrigger("Dead");
        }
    }
}
