using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// プレイヤーの基本操作を処理するコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    /// <summary>爆弾管理変数</summary>
    [SerializeField] int m_nowBomb = 1;
    /// <summary>s最大爆弾保持数</summary>
    [SerializeField] int m_maxBomb = 1;
    /// <summary>爆弾クールタイム</summary>
    [SerializeField] float BombCoolTime = 3;
    /// <summary>爆弾生成時間</summary>
    float BombTime;
    /// <summary>ダウンエフェクト</summary>
    [SerializeField] GameObject m_downEffect;
    /// <summary>移動スピード</summary>
    [SerializeField] int m_moveSpeed = 1;
    AudioSource m_audio;
    Rigidbody2D m_rb2d;
    PhotonView m_photonView;
    private Vector2 dir;
    private Vector2 posBomb;
    // Start is called before the first frame update
    void Start()
    {
        m_photonView = GetComponent<PhotonView>();
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
            dir = new Vector2(h, v).normalized; // 進行方向の単位ベクトルを作る (dir = direction)
            m_rb2d.velocity = dir * m_moveSpeed;        // 単位ベクトルにスピードをかけて速度ベクトルにして、それを Rigidbody の速度ベクトルとしてセットする
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Spaceキーが押されました");
                Bomb();
            }
        }
        
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
            PhotonNetwork.Instantiate("Bomb", posBomb, Quaternion.identity);//プレハブをインスタンス化する
            m_nowBomb -= 1;
        }  
    }
}
