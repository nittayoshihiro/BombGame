using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//using Photon.Realtime;

public class ItemManager : MonoBehaviour
{
    PhotonView m_photonView;
    // Start is called before the first frame update
    void Start()
    {
        m_photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("a");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("アイテム破棄");
            Destroy(this.gameObject);
        }
    }
}
