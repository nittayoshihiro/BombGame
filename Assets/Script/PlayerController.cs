using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
/// <summary>
/// プレイヤーの基本操作を処理するコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    /// <summary>爆弾のプレハブ</summary>
    [SerializeField] string m_bombPrefabName = "Prefab";
    /// <summary>爆弾管理変数</summary>
    [SerializeField] int m_nowBomb = 1;
    /// <summary>s最大爆弾保持数</summary>
    [SerializeField] int m_maxBomb = 1;
    /// <summary>爆弾クールタイム</summary>
    [SerializeField] float BombCoolTime = 3;
    /// <summary>爆弾生成時間</summary>
    float BombTime;
    /// <summary>爆弾を置けるか</summary>
    bool m_bomb = true;
    /// <summary>ダウンエフェクト</summary>
    [SerializeField] GameObject m_downEffect;
    /// <summary>移動スピード</summary>
    [SerializeField] float m_moveSpeed = 1;
    /// <summary>移動スピードを上げる変数 </summary>
    [SerializeField] float m_speedUp = 1.5f;
    /// <summary>直前に移動した方向</summary>
    Vector2 m_lastMovedDirection;
    bool m_isWalking = false;
    SpriteRenderer m_sprite;
    Animator m_anim;
    AudioSource m_audio;
    Rigidbody2D m_rb2d;
    PhotonView m_photonView;
    Collider2D m_collider2D;
    private Vector2 dir;
    private Vector2 posBomb;
    // Start is called before the first frame update
    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_photonView = GetComponent<PhotonView>();
        m_anim = GetComponent<Animator>();
        m_rb2d = GetComponent<Rigidbody2D>();
        m_audio = GetComponent<AudioSource>();
        m_nowBomb = m_maxBomb;//爆弾保持を最大にしておく
    }

    // Update is called once per frame
    void Update()
    {
        if (m_photonView.IsMine)
        {
            //Playerを移動させる
            float h = Input.GetAxisRaw("Horizontal");   // 垂直方向の入力を取得する
            float v = Input.GetAxisRaw("Vertical");     // 水平方向の入力を取得する

            Vector2 dir = AdjustInputDirection(h, v);   // 入力方向を４方向に変換（制限）する

            // オブジェクトを動かす
            m_rb2d.velocity = dir * m_moveSpeed;

            m_isWalking = dir == Vector2.zero ? false : true;

            m_anim.SetFloat("InputX", dir.x);
            m_anim.SetFloat("InputY", dir.y);
            m_anim.SetBool("IsWalking", m_isWalking);

            m_lastMovedDirection = dir;
            //爆弾リチャージ
            if (m_maxBomb > m_nowBomb)//爆弾が減ってるとき
            {
                BombTime += Time.deltaTime;
                if (BombTime > BombCoolTime)
                {
                    m_nowBomb += 1;//爆弾を一つ増やす
                    BombTime = 0;//ボムタイムをリセットする
                }
            }
            //爆弾設置
            if (m_bomb)//爆弾が置かれていないか
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Spaceキーが押されました");
                    Bomb();
                }
            }
        }
        
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
    /// <summary>
    /// 爆弾を設置する
    /// </summary>
    void Bomb()
    {
        //爆弾があれば設置する
        if (m_nowBomb > 0)
        {
            Debug.Log("爆弾設置");
            // X 座標と Y 座標を四捨五入
            posBomb = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));//爆弾の設置ポジション
            PhotonNetwork.Instantiate(m_bombPrefabName, posBomb, Quaternion.identity);//プレハブをインスタンス化する
            m_nowBomb -= 1;
        }  
    }
    void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (m_photonView.enabled)
        {
            if (collision2D.gameObject.tag == "Explosion")
            {
                Debug.Log("爆発死");
                PhotonNetwork.Destroy(gameObject);
            }
            if (collision2D.gameObject.tag == "Bomb")
            {
                Debug.Log("そこにはおけません");
                m_bomb = false;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision2D)
    {
        if (m_photonView.enabled)
        {
            if (collision2D.gameObject.tag == "Bomb")
            {
                Debug.Log("そこにはおけません");
                m_bomb = true;
                m_collider2D = collision2D.gameObject.GetComponent<Collider2D>();
                m_collider2D.isTrigger = false;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Speed")
        {
            m_moveSpeed = m_moveSpeed * m_speedUp;
            Debug.Log("現在のスピード" + m_moveSpeed);
        }
    }
}
