using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bomb : MonoBehaviour
{
    /// <summary>爆弾爆発のプレハブ</summary>
    [SerializeField] string m_explosionPrefabName = "Prefab";
    /// <summary>起爆時間</summary>
    [SerializeField] float DetonationTime = 3;
    /// <summary>爆弾のエフェクト</summary>
    [SerializeField] GameObject m_bombEffect;
    /// <summary>爆破範囲</summary>
    [SerializeField] int explosionRange = 1;
    /// <summary>ステージのレイヤー</summary>
    public LayerMask levelMask;
    /// <summary>タイマー</summary>
    float m_Time;
    PhotonView m_photonView;
    private Vector2 posExp;
    // Start is called before the first frame update
    void Start()
    {
        m_photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_photonView.IsMine)
        {
            m_Time += Time.deltaTime;
            if (m_Time > DetonationTime)//起爆時間を超えたら
            {
                //爆風を広げる
                StartCoroutine(Explosion(Vector2.up)); //上に広げる
                StartCoroutine(Explosion(Vector2.down)); //下に広げる
                StartCoroutine(Explosion(Vector2.right)); //右に広げる
                StartCoroutine(Explosion(Vector2.left)); //左に広げる
                PhotonNetwork.Destroy(gameObject);//このオブジェクトを破棄する
            }
        }
    }

    private IEnumerator Explosion(Vector2 direction)
    {
        Debug.Log("起爆");
        for (int i = 1; i < explosionRange; i++)
        {
            // ブロックとの当たり判定の結果を格納する変数
            RaycastHit2D hit;
            // 爆風を広げた先に何か存在するか確認
            hit = Physics2D.Raycast(transform.position, direction, i, levelMask);
            // 爆風を広げた先に何も存在しない場合
            if (!hit.collider)
            {
                Debug.Log("explosion");
                // 爆風を広げるために、
                // 爆発エフェクトのオブジェクトを作成
                PhotonNetwork.Instantiate(m_explosionPrefabName, 
                    new Vector2(transform.position.x,transform.position.y) + (direction *i)
                    , Quaternion.identity);//爆発を生成する
            }
            // 爆風を広げた先にブロックが存在する場合
            else
            {
                // 爆風はこれ以上広げない
                break;
            }
            
        }
        // 0.05 秒待ってから、次のマスに爆風を広げる
        yield return null;
    }
}
