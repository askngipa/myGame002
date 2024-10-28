using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float Speed;
    Rigidbody2D rig;
    SpriteRenderer spriter;
    Animator anim;
    
    private void Awake()
    {
        rig=GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim =GetComponent<Animator>();
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

    //LateUpdate : 프레임이 종료되기 전 실행되는 생명주기 함수
    private void LateUpdate()
    {
        //magnitude : 벡터의 순수한 크기 값
        anim.SetFloat("Speed",inputVec.magnitude);

        if (inputVec.x !=0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }
}
