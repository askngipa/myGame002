using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rig;
    Animator anim;
    SpriteRenderer spriter;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(!isLive)
            return;

        //��ġ���� = Ÿ����ġ - ������ġ
        Vector2 dirVec = target.position - rig.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.deltaTime;

        //�÷��̾��� Ű�Է� ���� ���� �̵� = ������ ���Ⱚ�� ���� �̵�
        rig.MovePosition(rig.position + nextVec);

        //�����ӵ��� �̵��� ������ ���� �ʵ��� �ӵ� ����.
        rig.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        //��ǥ�� x��� �ڽ��� x���� ���� ���Ͽ� ������ true�� �ǵ��� ����
        spriter.flipX = target.position.x < rig.position.x;
    }

    //OnEnable���� Ÿ�� ������ ���ӸŴ����� Ȱ���Ͽ� �÷��̾ �Ҵ�
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true; //�������ο� ü�� �ʱ�ȭ
        health = maxHealth; //�ʱ�ȭ
    }

    //�ʱ� �Ӽ��� �����ϴ� �Լ� �߰�
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    //OnTriggerEnter2D �Ű������� �±��������� Ȱ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        //������Ʈ"bullet"���� �����Ͽ� �������� ������ �ǰ� ���
        health -= collision.GetComponent<Bullet>().damage;

        //����ü���� �������� �ǰݰ� ������� �������� ������
        if (health > 0)
        {
            //Live, Hit, Action
        }
        else
        {
            //die..
            Dead();
        }

    }

    private void Dead()
    {
        //����Ҷ� SetActive �Լ��� ���� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
