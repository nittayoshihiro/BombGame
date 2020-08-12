using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bomb : MonoBehaviour
{
    /// <summary>起爆時間</summary>
    [SerializeField] float DetonationTime = 3;
    /// <summary>爆弾のエフェクト</summary>
    [SerializeField] GameObject m_bombEffect;
    /// <summary>爆弾範囲のobject/summary>
    //[SerializeField] GameObject m_explosionRange;
    /// <summary>タイマー</summary>
    float m_Time;
    private Vector2 posExp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time > DetonationTime)//起爆時間を超えたら
        {
            Debug.Log("起爆");
            //Instantiate(m_bombEffect, gameObject.transform.position, m_bombEffect.transform.rotation);//プレハブをインスタンス化する
            Destroy(this.gameObject);//このオブジェクトを破棄する
            posExp = new Vector2 (this.transform.position.x, this.transform.position.y);
            PhotonNetwork.Instantiate("ExplosionRange", posExp , Quaternion.identity);//爆発範囲を生成する
        }
    }
}
