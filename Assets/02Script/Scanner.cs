using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    //����, ���̾�, ��ĵ����迭, ���尡��� ��ǥ�� ��������
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        //CircleCastAll : ������ ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ�
        //1.ĳ���� ������ġ
        //2.���� ������
        //3.ĳ���� ����
        //4.ĳ���� ����
        //5.��� ���̾�
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        //���������� ������Ʈ
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        //foreach������ ĳ���� ��� ������Ʈ�� �ϳ��� ����
        foreach (RaycastHit2D target in targets)
        {
            Vector3 mypos = transform.position;
            Vector3 targetPos = target.transform.position;

            //Distance(A, B) : ���� A �� B�� �Ÿ��� ������ִ� �Լ�
            float curDiff = Vector3.Distance(mypos, targetPos);

            //�ݺ��� ���ٰ� ������ �Ÿ��� ����� �Ÿ����� ������ ��ü
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }

}
