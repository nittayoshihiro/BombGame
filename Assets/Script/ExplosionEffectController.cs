using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExplosionEffectController : MonoBehaviour
{
    [SerializeField] float animationTime = 1.0f;
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
            if (m_Time > animationTime)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
       
    }
}
