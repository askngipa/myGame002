using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //����ID, ������ID, ������, ����, �ӵ� ����
    public int ID;
    public int prefabID;
    public float damage;
    public int count;
    public float speed;

    //Ÿ�̸�
    float timer;

    //�÷��̾� ����
    Player player;

    private void Awake()
    {
        //GetComponentInParent : �Լ��� �θ��� ������Ʈ ��������
        //player = GetComponentInParent<Player>();
        player = GameManager.instance.player;
    }

    private void Update()
    {
        switch (ID)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                timer += Time.deltaTime;

                //speed���� Ŀ���� �ʱ�ȭ�ϸ鼭 �߻���� ����
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }

    public void LevelUp(float damage,int count)
    {
        this.damage = damage;
        this.count += count;

        if (ID == 0)
        {
            Batch();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);

    }

    //Weapon �ʱ�ȭ �Լ��� ��ũ���ͺ� ������Ʈ�� �Ű������� �޾� Ȱ��
    public void Init(ItemData data)
    {
        //Basic Set
        name = "Weapon " + data.itemID;
        transform.parent = player.transform;

        //������ġ�� localPosition�� �������� ����
        transform.localPosition = Vector3.zero;

        //Property Set
        //�ʱ�ȭ
        ID = data.itemID;
        damage = data.baseDamage;
        count = data.baseCount;
        
        for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabID =i;
                break;
            }
        }

        switch (ID)
        {
            case 0:
                speed = 150;
                Batch();
                break;

            default:
                //speed���� ����ӵ��� �ǹ�: �������� ���� �߻�
                speed = 0.4f;
                break;
        }

        //Hand Set
        //enum �� �տ� intŸ���� �ۼ��Ͽ� ���� ����ȯ
        Hand hand = player.hands[(int)data.itemType];

        hand.spriter.sprite = data.hand;

        hand.gameObject.SetActive(true);

        //BroadcastMessage : Ư�� �Լ� ȣ���� ��� �ڽĿ��� ����ϴ� �Լ�
        //BroadcastMessage �� �ι�° ���ڰ����� DontRequireReceiver �߰�
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }

    //������ ���⸦ ��ġ�ϴ� �Լ����� �� ȣ��
    private void Batch()
    {
        //for������ count��ŭ Ǯ������ ��������
        for(int i = 0; i < count; i++)
        {
            //�⺻ ������Ʈ�� ���� Ȱ���ϰ� ���ڶ� ���� Ǯ������ ��������.
            Transform bullet; 
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabID).transform;

                //paerent �Ӽ��� ���� �θ� ����
                bullet.parent = transform;
            }

            //�ʱ�ȭ�Լ�
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;

            //Rotate : �Լ��� ���� ����
            bullet.Rotate(rotVec);

            //Translate : �Լ��� �ڽ��� �������� �̵�
            //Space.World ���� 
            bullet.Translate(bullet.up * 1.5f,Space.World);

            //�Ӽ� �ʱ�ȭ�Լ�
            bullet.GetComponent<Bullet>().Init(damage, -1,Vector3.zero); // -1 is Infinity per.
        }
    }

    private void Fire()
    {
        //������ ��ǥ�� ������ �Ѿ�� ����
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;

        //ũ�Ⱑ ���Ե� ���� : ��ǥ��ġ - ���� ��ġ
        Vector3 dir = targetPos - transform.position;

        //normalized : ���� ������ ������ �����ϰ� ũ�⸦ 1�� ���ѵ� �Ӽ�
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabID).transform;

        //���� ���� ������ �״�� Ȱ���ϸ鼭 ��ġ�� �÷��̾� ��ġ�� ����
        bullet.position = transform.position;

        //FromToRotation : ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        //���Ÿ� ���ݿ� �°� �ʱ�ȭ �Լ� ȣ���ϱ�
        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }

}
