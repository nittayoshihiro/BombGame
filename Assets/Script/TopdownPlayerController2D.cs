using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class TopdownPlayerController2D : MonoBehaviour
{
    /// <summary>移動速度</summary>
    [SerializeField] float m_walkSpeed = 1f;
    /// <summary>直前に移動した方向</summary>
    Vector2 m_lastMovedDirection;
    SpriteRenderer m_sprite;
    Animator m_anim;
    Rigidbody2D m_rb;
    bool m_isWalking = false;

    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 dir = AdjustInputDirection(h, v);   // 入力方向を４方向に変換（制限）する
        // オブジェクトを動かす
        //transform.Translate(m_walkSpeed * dir * Time.deltaTime);    // このやり方でもできるが、コライダーにめり込む
        m_rb.velocity = dir * m_walkSpeed;

        // 入力方向によって左右の向きを変える
        if (dir.x != 0)
        {
            m_sprite.flipX = (dir.x > 0);
        }

        m_isWalking = dir == Vector2.zero ? false : true;

        m_anim.SetFloat("InputX", dir.x);
        m_anim.SetFloat("InputY", dir.y);
        m_anim.SetBool("IsWalking", m_isWalking);

        m_lastMovedDirection = dir;
    }

    /// <summary>
    /// 入力された方向を４方向に制限し、Vector2 にして返す
    /// （斜めに入力された場合でも、それ以前の入力状況に応じて４方向に制限する）
    /// </summary>
    /// <param name="inputX"></param>
    /// <param name="inputY"></param>
    Vector2 AdjustInputDirection(float inputX, float inputY)
    {
        Vector2 dir = new Vector2(inputX, inputY);

        if (m_lastMovedDirection == Vector2.zero)
        {
            if (dir.x != 0 && dir.y != 0)
            {
                dir.y = 0;
            }
        }
        else if (m_lastMovedDirection.x != 0)
        {
            dir.y = 0;
        }
        else if (m_lastMovedDirection.y != 0)
        {
            dir.x = 0f;
        }

        return dir;
    }
}
