using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Explosion : MonoBehaviour
{
    /// <summary>爆発範囲が消滅するまでの時間</summary>
    [SerializeField] float finishExplosionTime = 1;
    /// <summary>時間</summary>
    float m_Time;
    PhotonView m_photonView;
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
            if (m_Time > finishExplosionTime)
            {
                PhotonNetwork.Destroy(gameObject);//このオブジェクトを破棄する
            }
        } 
    }
}
