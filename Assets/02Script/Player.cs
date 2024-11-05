using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Vector2 nextVec = inputVec * Speed * Time.fixedDeltaTime;
        rig.MovePosition(rig.position + nextVec);
    }

    private void OnMove (InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    //LateUpdate : �������� ����Ǳ� �� ����Ǵ� �����ֱ� �Լ�
    private void LateUpdate()
    {
        //magnitude : ������ ������ ũ�� ��
        anim.SetFloat("Speed",inputVec.magnitude);

        if (inputVec.x !=0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
