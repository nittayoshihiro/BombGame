using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//using Photon.Realtime;

public class BrokenBlock : MonoBehaviour
{

    PhotonView m_photonView;
    // Start is called before the first frame update
    void OnEnable()
    {
        m_photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (m_photonView.enabled)
        {
            if (collision2D.gameObject.tag == "Explosion")
            {
                Debug.Log("aiueo");
                Destroy(gameObject);
            }
        }
        
    }
}
