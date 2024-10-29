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
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    //�ʱ�ȭ
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        //GetCurrentAnimatorStateInfo : ���� ���� ������ ���������Լ�
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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
        coll.enabled = true;
        rig.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
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
        StartCoroutine(KnockBack());

        //����ü���� �������� �ǰݰ� ������� �������� ������
        if (health > 0)
        {
            //Live, Hit, Action
            //�ǰ� �κп� �ִϸ��̼� SetTrigget�Լ��� ȣ���Ͽ� ���� ����
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;

            //������Ʈ ��Ȱ��ȭ�� .enabled = false;
            coll.enabled = false;

            //������ٵ��� ������ ��Ȱ��ȭ�� .simulated = false;
            rig.simulated = false;

            //��������Ʈ �������� sortingOrder ����
            //order in layer ����
            spriter.sortingOrder = 1;

            //SetBool�Լ��� ���� �״� �ִϸ��̼� ���·� ��ȯ
            anim.SetBool("Dead", true);

            Dead();
        }

    }

    //�ڷ�ƾ Coroutine : �����ֱ�� �񵿱�ó�� ����Ǵ� �Լ�
    //IEnumerator : �ڷ�ƾ���� ��ȯ�� �������̽�
    IEnumerator KnockBack()
    {
        //yield : �ڷ�ƾ�� ��ȯ Ű����
        //yield return �� ���� �پ��� ���½ð��� ����
        //yield return null; //1������ ���� //�ϳ��� ���� �������� ������
        //yield return new WaitForSeconds(2f); //2�� ����

        yield return wait;//���� �ϳ��� ���������� ������
        Vector3 playerPos = GameManager.instance.player.transform.position;

        //�÷��̾� ������ �ݴ���� : ������ġ - �÷��̾� ��ġ
        Vector3 dirVec = transform.position - playerPos;

        //������ٵ�2D�� AddForce�Լ��� �� ���ϱ�
        //�������� ���̹Ƿ� ForceMode2D.Impulse�Ӽ� �߰�.
        rig.AddForce(dirVec.normalized *3,ForceMode2D.Impulse);

    }

    private void Dead()
    {
        //����Ҷ� SetActive �Լ��� ���� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
