using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    /// <summary>起爆時間</summary>
    [SerializeField] float DetonationTime = 3;
    /// <summary>爆弾のエフェクト</summary>
    [SerializeField] GameObject m_bombEffect;
    /// <summary>タイマー</summary>
    float m_Time;
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
        }
    }
}
